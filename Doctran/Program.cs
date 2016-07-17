// <copyright file="Program.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Xml.Linq;
    using Helper;
    using Reporting;
    using Utilitys;

    public partial class Program
    {
        public static bool ShowLicensing { get; private set; }

        private static Stopwatch StageStopwatch { get; } = new Stopwatch();

        private static XElement TimingXml { get; } = new XElement("timings", new XElement("description", "Times are given in milliseconds."));

        private static Stopwatch TotalStopwatch { get; } = new Stopwatch();

        public static int Main(string[] args)
        {
#if DEBUG
            Report.SetReleaseProfile();
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
            var sourceDocument = CreateSourceXml(project);
            if (options.SaveXmls)
            {
                xmlOutputter.SaveToDisk(EnvVar.XmlOutputPath(options.OutputDirectory, "project.xml"));
                sourceDocument.Save(EnvVar.XmlOutputPath(options.OutputDirectory, "source.xml"), SaveOptions.DisableFormatting);
            }

            if (options.NoOutput)
            {
                return 0;
            }

            OutputTheme(options);
            OutputHtml(xmlOutputter, sourceDocument, options);

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
    }
}