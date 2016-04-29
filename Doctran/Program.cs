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
    using System.Diagnostics;
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

        private static Stopwatch StageStopwatch { get; } = new Stopwatch();

        private static XElement TimingXml { get; } = new XElement("timings", new XElement("description", "Times are given in milliseconds."));

        private static Stopwatch TotalStopwatch { get; } = new Stopwatch();

        public static int Main(string[] args)
        {
#if DEBUG
            Report.SetDebugProfile();
#endif

#if !DEBUG
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            Report.SetReleaseProfile();
#endif

            TotalStopwatch.Restart();
            StageStopwatch.Restart();
            var options = new Options();
            GetCommandLineOptions(args, options);
            GetOptions(options.ProjectFilePath ?? EnvVar.DefaultInfoPath, options);
            Report.Verbose = options.Verbose;
            SaveTiming("options-parsing", StageStopwatch.ElapsedMilliseconds);

            var project = GetProject(options.SourceFilePaths, options.RunInSerial);

            var xmlOutputter = GetXmlOutputter(project, new XElement("Information", options.XmlInformation));
            if (options.SaveXmls)
            {
                xmlOutputter.SaveToDisk(EnvVar.XmlOutputPath(options.OutputDirectory, "project.xml"));
            }

            if (options.NoOutput)
            {
                return 0;
            }

            OutputTheme(options);
            OutputHtml(project, xmlOutputter, options);

            SaveTiming("total", TotalStopwatch.ElapsedMilliseconds);
            if (options.TimeOutput)
            {
                Directory.CreateDirectory(EnvVar.XmlOutputPath(options.OutputDirectory));
                TimingXml.Save(EnvVar.XmlOutputPath(options.OutputDirectory, "timings.xml"));
            }

            Report.NewStatus($@"Documentation can be found at '{Path.GetFullPath(options.OutputDirectory)}'");
            Report.NewStatus("Documentation generation complete.\n");
            return 0;
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
                new XDocument(new XElement("Source", xElements)).Save(EnvVar.XmlOutputPath(options.OutputDirectory, "source.xml"));
            }

            return reader;
        }

        private static void GetCommandLineOptions(string[] args, Options options)
        {
            Parser.Default.ParseArgumentsStrict(args, options);

            ShowLicensing = options.ShowLicensing;

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
            var projFileSource = OtherUtils.ReadAllText(path);

            PathUtils.RunInDirectory(
                Path.GetDirectoryName(path),
                () => projectFileReader.Parse(options, path, projFileSource));
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

        private static Project GetProject(IEnumerable<string> sourceFiles, bool runInSerial)
        {
            StageStopwatch.Restart();
            Report.NewStatus("Analysing project block structure... ");
            var proj = ProgramHelper.ParseProject(sourceFiles, runInSerial);

            SaveTiming("project-parsing", StageStopwatch.ElapsedMilliseconds);
            Report.ContinueStatus("Done");
            return proj;
        }

        private static XmlOutputter GetXmlOutputter(Project project, XElement xmlInformation)
        {
            StageStopwatch.Restart();
            Report.NewStatus("Generating xml... ");
            var xmlOutputter = new XmlOutputter(project.XEle(xmlInformation));

            SaveTiming("xml-generation", StageStopwatch.ElapsedMilliseconds);
            Report.ContinueStatus("Done");
            return xmlOutputter;
        }

        private static void OutputHtml(Project project, XmlOutputter xmlOutputter, Options options)
        {
            StageStopwatch.Restart();
            Report.NewStatus("Generating htmls... ");

            var reader = CreateSourceXml(project, options);

            var preProcess = new XsltRunner(Path.Combine(EnvVar.ExecPath, "themes", options.ThemeName, "main_pre.xslt"));
            var preProcessResult = preProcess.Run(xmlOutputter.XDocument, Path.GetFullPath(options.OutputDirectory));

            if (options.SaveXmls)
            {
                preProcessResult.Save(EnvVar.XmlOutputPath(options.OutputDirectory, "documentation_file.xml"));
            }

            var htmlOutputter = new XsltRunner(Path.Combine(EnvVar.ExecPath, "themes", options.ThemeName, "main.xslt"));

            htmlOutputter.Run(
                preProcessResult.ToXDocument(),
                Path.GetFullPath(options.OutputDirectory) + EnvVar.Slash,
                new KeyValuePair<string, object>("verbose", Report.Verbose),
                new KeyValuePair<string, object>("source", reader));

            SaveTiming("html-output", StageStopwatch.ElapsedMilliseconds);
            Report.ContinueStatus("Done");
        }

        private static void OutputTheme(Options options)
        {
            StageStopwatch.Restart();
            Report.NewStatus("Outputting theme files... ");
            var themeParts = DocumentationManager.RequiredThemeParts(options.SourceFilePaths.Select(Path.GetExtension));
            var themeOutputter = new AssetOutputter(themeParts);
            themeOutputter.Output(options.OverwriteExisting, options.OutputDirectory, options.ProjectFilePath, options.ThemeName, options.CopyPaths, options.CopyAndParsePaths);

            SaveTiming("theme-output", StageStopwatch.ElapsedMilliseconds);
            Report.ContinueStatus("Done");
        }

        private static void SaveTiming(string name, long milliseonds)
        {
            TimingXml.Add(new XElement(name, milliseonds));
        }

        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var ex = (Exception)e.ExceptionObject;

                var logPath = Path.GetFullPath("errors.log");
                Console.WriteLine("\ninternal error");
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