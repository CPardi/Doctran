// <copyright file="Program.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Helper;
    using Input.OptionFile;
    using Output;
    using Output.Themes;
    using Parsing.BuiltIn.FortranObjects;
    using Plugins;
    using Reporting;
    using Utilitys;
    using Parser = CommandLine.Parser;
    
    public class Program
    {
        public static bool ShowLicensing { get; private set; }

        public static int Main(string[] args)
        {
#if RELEASE
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
#endif
            
            Report.SetReleaseProfile();

            var options = GetOptions(args);
            options.SourceFilePaths.KeepDistinctOnly();
            Report.Verbose = options.Verbose;

            var project = GetProject(options.SourceFilePaths);

            OutputTheme(options);
            var xmlOutputter = GetXmlOutputter(project, options.XmlInformation, options.OutputDirectory, options.SaveXmlPath);
            OutputHtml(project, xmlOutputter, options);

            // Verbose >= 2
            Report.NewStatus($@"Documentation can be found at '{Path.GetFullPath(options.OutputDirectory)}'");
            Report.NewStatus("Documentation generation complete.\n");
            return 0;
        }

        private static Options GetOptions(string[] args)
        {
            var options = new Options();

            try
            {
                Parser.Default.ParseArgumentsStrict(args, options);
            }
            catch (IOException e)
            {
                Report.Error(pub => pub.DescriptionReason(ReportGenre.Argument, e.Message), e);
            }

            ShowLicensing = options.ShowLicensing;

            PluginManager.Initialize();

            if (options.ShowHelp)
            {
                Report.MessageAndExit(options.GetUsage());
            }

            if (options.ShowPluginInformation)
            {
                Report.MessageAndExit(PluginManager.InformationString);
            }

            var fileParser = new Parser<Options>();
            fileParser.AddRecognisedOption("Source");
            fileParser.AddRecognisedOption("Copy");
            fileParser.AddRecognisedOption("CopyAndParse");
            fileParser.AddRecognisedOption("UserPage", new UserPageFactory());
            fileParser.AddRecognisedOption("Menu", new MenuFactory());

            fileParser.ParseFile(options.ProjectFilePath ?? EnvVar.DefaultInfoPath, options);

            return options;
        }

        private static Project GetProject(IEnumerable<string> sourceFiles)
        {            
            Report.NewStatus("Analysing project block structure... ");            
            var proj = ProgramHelper.ParseProject(sourceFiles);
            Report.ContinueStatus("Done");
            return proj;
        }

        private static XmlOutputter GetXmlOutputter(Project project, XElement xmlInformation, string outputDirectory, string saveXmlPath)
        {
            Report.NewStatus("Generating xml... ");   
                     
            var xmlOutputter = new XmlOutputter(project.XEle(xmlInformation));
            if (saveXmlPath != null)
            {
                xmlOutputter.SaveToDisk(Path.Combine(outputDirectory, saveXmlPath));
            }

            Report.ContinueStatus("Done");
            return xmlOutputter;
        }

        private static string ModXmlPath(string saveXmlPath)
        {
            var ext = Path.GetExtension(saveXmlPath);
            return saveXmlPath.Substring(0, saveXmlPath.Length - ext?.Length ?? 0) + "mod" + ext;
        }

        private static void OutputHtml(Project project, XmlOutputter xmlOutputter, Options options)
        {
            Report.NewStatus("Generating htmls... ");

            var xElements =
                from source in project.Sources
                let highlighter = DocumentationManager.TryGetDefinitionByIdentifier(source.Language)
                select new XElement("File",
                    new XElement("Identifier", source.Identifier),
                    highlighter.HighlightLines(source.OriginalLines));

            var reader = new XDocument(new XElement("Source", xElements)).CreateReader();

            new XDocument(new XElement("Source", xElements)).Save(Path.Combine(options.OutputDirectory, "source.xml"));

            var preProcess = new XsltRunner(Path.Combine(EnvVar.ExecPath, "themes", options.ThemeName, "main_pre.xslt"));
            var preProcessResult = preProcess.Run(xmlOutputter.XDocument, Path.GetFullPath(options.OutputDirectory));

            preProcessResult.Save(Path.Combine(options.OutputDirectory, ModXmlPath(options.SaveXmlPath)));

            var htmlOutputter = new XsltRunner(Path.Combine(EnvVar.ExecPath, "themes", options.ThemeName, "main.xslt"));

            htmlOutputter.Run(
                preProcessResult.ToXDocument(),
                Path.GetFullPath(options.OutputDirectory) + EnvVar.Slash,
                new KeyValuePair<string, object>("verbose", Report.Verbose),
                new KeyValuePair<string, object>("source", reader));

            Report.ContinueStatus("Done");
        }

        private static void OutputTheme(Options options)
        {
            Report.NewStatus("Outputting theme files... ");
            var themeOutputter = new ThemeOutputter();
            themeOutputter.Output(options);
            Report.ContinueStatus("Done");
        }

        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var ex = (Exception)e.ExceptionObject;

                var logPath = Path.GetFullPath("errors.log");
                Console.WriteLine("internal error");
                Console.WriteLine("Exception details written to " + logPath);
                using (var sq = new StreamWriter(logPath, false))
                {
                    sq.WriteLine("======Exception Type======");
                    sq.WriteLine(ex.GetType().Name);
                    sq.WriteLine(string.Empty);

                    sq.WriteLine("======Message======");
                    sq.WriteLine(ex.Message);
                    sq.WriteLine(string.Empty);

                    sq.WriteLine("======StackTrace======");
                    sq.WriteLine(ex.StackTrace);
                    sq.WriteLine(string.Empty);

                    sq.WriteLine("======Data======");
                    sq.WriteLine(ex.Data);
                    sq.WriteLine(string.Empty);

                    sq.WriteLine("======InnerException======");
                    sq.WriteLine(ex.InnerException);
                    sq.WriteLine(string.Empty);

                    sq.WriteLine("======Source======");
                    sq.WriteLine(ex.Source);
                    sq.WriteLine(string.Empty);
                }
            }
            finally
            {
                Environment.Exit(1);
            }
        }
    }
}