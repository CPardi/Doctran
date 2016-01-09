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
    using System.Xml;
    using System.Xml.Linq;
    using CommandLine;
    using Helper;
    using Input.Options;
    using Output.Assets;
    using Output.Html;
    using ParsingElements.FortranObjects;
    using Plugins;
    using Reporting;
    using Utilitys;

    public class Program
    {
        public static bool ShowLicensing { get; private set; }

        public static int Main(string[] args)
        {
#if RELEASE
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
#endif

            Report.SetReleaseProfile();

            var options = new Options();
            GetCommandLineOptions(args, options);
            GetOptions(options.ProjectFilePath ?? EnvVar.DefaultInfoPath, options);
            Report.Verbose = options.Verbose;

            var project = GetProject(options.SourceFilePaths);

            OutputTheme(options);
            var xmlOutputter = GetXmlOutputter(project, new XElement("Information", options.XmlInformation));
            if (options.SaveXmls)
            {
                xmlOutputter.SaveToDisk(EnvVar.XmlOutputDirectory(options.OutputDirectory, "project.xml"));
            }

            OutputHtml(project, xmlOutputter, options);

            Report.NewStatus($@"Documentation can be found at '{Path.GetFullPath(options.OutputDirectory)}'");
            Report.NewStatus("Documentation generation complete.\n");
            return 0;
        }

        private static void GetCommandLineOptions(string[] args, Options options)
        {
            Parser.Default.ParseArgumentsStrict(args, options);

            Program.ShowLicensing = options.ShowLicensing;

            PluginManager.Initialize(); // Must come after show licensing.

            if (options.ShowPluginInformation)
            {
                Report.MessageAndExit(PluginManager.InformationString);
            }
        }

        private static void GetOptions(string path, Options options)
        {
            var parserExceptions = new ListenerAndAggregater<OptionReaderException>();
            var projectFileReader = new OptionsReader<Options>(5, "Project file") { ErrorListener = parserExceptions };
            var projFileLines = OtherUtils.ReadFile(path);
            PathUtils.RunInDirectory(
                Path.GetDirectoryName(path),
                () => projectFileReader.Parse(options, path, projFileLines));
            var ws = parserExceptions.Warnings;
            var es = parserExceptions.Errors;

            // Report any errors to the user.
            Action<ConsolePublisher, OptionReaderException> action
                = (p, e) => p.DescriptionReasonLocation(ReportGenre.ProjectFile, e.Message, StringUtils.LocationString(e.StartLine, e.EndLine, Path.GetFullPath(path)));

            if (ws.Any())
            {
                Report.Warnings(action, ws);
            }

            if (es.Any())
            {
                Report.Errors(action, es);
            }
        }

        private static Project GetProject(IEnumerable<string> sourceFiles)
        {
            Report.NewStatus("Analysing project block structure... ");
            var proj = ProgramHelper.ParseProject(sourceFiles);
            Report.ContinueStatus("Done");
            return proj;
        }

        private static XmlOutputter GetXmlOutputter(Project project, XElement xmlInformation)
        {
            Report.NewStatus("Generating xml... ");
            var xmlOutputter = new XmlOutputter(project.XEle(xmlInformation));
            Report.ContinueStatus("Done");
            return xmlOutputter;
        }

        private static void OutputHtml(Project project, XmlOutputter xmlOutputter, Options options)
        {
            Report.NewStatus("Generating htmls... ");

            var reader = CreateSourceXml(project, options);

            var preProcess = new XsltRunner(Path.Combine(EnvVar.ExecPath, "themes", options.ThemeName, "main_pre.xslt"));
            var preProcessResult = preProcess.Run(xmlOutputter.XDocument, Path.GetFullPath(options.OutputDirectory));

            if (options.SaveXmls)
            {
                preProcessResult.Save(EnvVar.XmlOutputDirectory(options.OutputDirectory, "documentation_file.xml"));
            }

            var htmlOutputter = new XsltRunner(Path.Combine(EnvVar.ExecPath, "themes", options.ThemeName, "main.xslt"));

            htmlOutputter.Run(
                preProcessResult.ToXDocument(),
                Path.GetFullPath(options.OutputDirectory) + EnvVar.Slash,
                new KeyValuePair<string, object>("verbose", Report.Verbose),
                new KeyValuePair<string, object>("source", reader));

            Report.ContinueStatus("Done");
        }

        private static XmlReader CreateSourceXml(Project project, Options options)
        {
            var xElements =
                from source in project.Sources
                let highlighter = DocumentationManager.TryGetDefinitionByIdentifier(source.Language)
                select new XElement(
                    "File",
                    new XElement("Identifier", source.Identifier),
                    highlighter.HighlightLines(source.OriginalLines));
            var reader = new XDocument(new XElement("Source", xElements)).CreateReader();
            if (options.SaveXmls)
            {
                new XDocument(new XElement("Source", xElements)).Save(EnvVar.XmlOutputDirectory(options.OutputDirectory, "source.xml"));
            }

            return reader;
        }

        private static void OutputTheme(Options options)
        {
            Report.NewStatus("Outputting theme files... ");
            var themeOutputter = new AssetOutputter();
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