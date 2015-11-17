//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

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
    using Parsing;
    using Parsing.FortranObjects;
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

            Report.SetDebugProfile();

            var options = GetOptions(args);
            
            options.SourceFilePaths.KeepDistinctOnly();
            EnvVar.Verbose = options.Verbose;
           
            var project = GetProject(options.SourceFilePaths);

            OutputTheme(options);
            var xmlOutputter = GetXmlOutputter(project, options.XmlInformation, options.OutputDirectory, options.SaveXmlPath);
            //OutputHtml(project, xmlOutputter, options);

            if (EnvVar.Verbose >= 2) Console.WriteLine("Done");
            if (EnvVar.Verbose >= 2) Console.WriteLine(@"Documentation can be found at """ + Path.GetFullPath(options.OutputDirectory) + @"""");
            if (EnvVar.Verbose >= 2) Console.WriteLine(@"Documentation generation complete.");
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
                Report.Error(
                    (pub, ex) =>
                    {
                        pub.AddErrorDescription("Invalid argument list.");
                        pub.AddReason(e.Message);
                    }, e);
            }

            ShowLicensing = options.ShowLicensing;

            PluginManager.Initialize();

            if (options.ShowHelp)
            {
                Report.MessageThenExit(
                    pub =>
                    {
                        pub.AddMessage(options.GetUsage());
                    });
            }

            if (options.ShowPluginInformation)
            {
                Report.MessageThenExit(
                    pub =>
                    {
                        pub.AddMessage(PluginManager.InformationString);
                    });
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

        private static Project2 GetProject(IEnumerable<string> sourceFiles)
        {
            if (EnvVar.Verbose >= 2) Console.Write("Analysing project block structure... ");
            return ProgramHelper.ParseProject(sourceFiles);
        }

        private static XmlOutputter GetXmlOutputter(Project2 project, XElement xmlInformation, string outputDirectory, string saveXmlPath)
        {
            if (EnvVar.Verbose >= 2) Console.Write("Done" + Environment.NewLine + "Generating xml... ");
            var xmlOutputter = new XmlOutputter(project.XEle(xmlInformation));
            if (saveXmlPath != null)
            {
                xmlOutputter.SaveToDisk(Path.Combine(outputDirectory, saveXmlPath));
            }
            return xmlOutputter;
        }

        private static void OutputHtml(FortranObject project, XmlOutputter xmlOutputter, Options options)
        {
            var xElements = 
                (from file in project.SubObjectsOfType<SourceFile>()
                select file.SourceXEle).ToList();
            var reader = new XDocument(new XElement("Source", xElements)).CreateReader();

            if (EnvVar.Verbose >= 2) Console.Write("Done" + Environment.NewLine + "Generating htmls... ");

            var preProcess = new XsltRunner(Path.Combine(EnvVar.ExecPath, "themes", options.ThemeName, "main_pre.xslt"));
            var preProcessResult = preProcess.Run(xmlOutputter.XDocument, Path.GetFullPath(options.OutputDirectory));

            var htmlOutputter = new XsltRunner(Path.Combine(EnvVar.ExecPath, "themes", options.ThemeName, "main.xslt"));

            htmlOutputter.Run(preProcessResult.ToXDocument(),
                Path.GetFullPath(options.OutputDirectory) + EnvVar.Slash,
                new KeyValuePair<string, object>("verbose", EnvVar.Verbose),
                new KeyValuePair<string, object>("source", reader));
        }

        private static void OutputTheme(Options options)
        {
            if (EnvVar.Verbose >= 2) Console.Write("Done" + Environment.NewLine + "Outputting theme files... ");
            var themeOutputter = new ThemeOutputter();
            themeOutputter.Output(options);
        }
        
        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var ex = (Exception) e.ExceptionObject;

                var logPath = Path.GetFullPath("errors.log");
                Console.WriteLine("internal error");
                Console.WriteLine("Exception details written to " + logPath);
                using (var sq = new StreamWriter(logPath, false))
                {
                    sq.WriteLine("======Exception Type======");
                    sq.WriteLine(ex.GetType().Name);
                    sq.WriteLine("");

                    sq.WriteLine("======Message======");
                    sq.WriteLine(ex.Message);
                    sq.WriteLine("");

                    sq.WriteLine("======StackTrace======");
                    sq.WriteLine(ex.StackTrace);
                    sq.WriteLine("");

                    sq.WriteLine("======Data======");
                    sq.WriteLine(ex.Data);
                    sq.WriteLine("");

                    sq.WriteLine("======InnerException======");
                    sq.WriteLine(ex.InnerException);
                    sq.WriteLine("");

                    sq.WriteLine("======Source======");
                    sq.WriteLine(ex.Source);
                    sq.WriteLine("");
                }
            }
            finally
            {
                Environment.Exit(1);
            }
        }
    }
}