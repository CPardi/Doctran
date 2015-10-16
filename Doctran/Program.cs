//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Doctran
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using BaseClasses;
    using Fbase.Common;
    using Fbase.Outputters;
    using Fbase.Projects;
    using OptionFile;
    using File = Fbase.Files.File;
    using Parser = CommandLine.Parser;

    public class Program
    {
        public static bool ShowLicensing { get; private set; }

        public static int Main(string[] args)
        {
#if RELEASE
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
#endif

            var options = new Options();

            try
            {
                Parser.Default.ParseArguments(args, options);
            }
            catch (IOException e)
            {
                UserInformer.GiveError("command line arguments", e.Message);
            }

            if (options.ShowHelp)
            {
                Console.WriteLine(options.GetUsage());
                Helper.Stop();
            }

            if (options.ShowPluginInformation)
            {
                PluginManager.WriteInformation();
                Helper.Stop();
            }

            ShowLicensing = options.ShowLicensing;

            PluginManager.Initialize();

            var fileParser = new Parser<Options>();
            fileParser.AddRecognisedOption("Source");
            fileParser.AddRecognisedOption("Copy");
            fileParser.AddRecognisedOption("CopyAndParse");
            fileParser.AddRecognisedOption("UserPage", new UserPageFactory());
            fileParser.AddRecognisedOption("Menu", new MenuFactory());

            fileParser.ParseFile(options.ProjectFilePath ?? EnvVar.defaultInfoPath, options);

            options.SourceFilePaths.KeepDistinctOnly();
            EnvVar.Verbose = options.Verbose;

            var project = GetProject(options);
            PostProcessProject(ref project);

            OutputTheme(options);
            var xmlOutputter = GetXmlOutputter(project, options);
            OutputHtml(project, xmlOutputter, options);

            if (EnvVar.Verbose >= 2) Console.WriteLine("Done");
            if (EnvVar.Verbose >= 2) Console.WriteLine(@"Documentation can be found at """ + Path.GetFullPath(options.OutputDirectory) + @"""");
            if (EnvVar.Verbose >= 2) Console.WriteLine(@"Documentation generation complete.");
            return 0;
        }

        private static Project GetProject(Options options)
        {
            if (EnvVar.Verbose >= 2) Console.Write("Analysing project block structure... ");
            return new Project(options);
        }

        private static XmlOutputter GetXmlOutputter(Project project, Options options)
        {
            if (EnvVar.Verbose >= 2) Console.Write("Done" + Environment.NewLine + "Generating xml... ");
            var xmlOutputter = new XmlOutputter(project.XEle());
            if (options.SaveXmlPath != null)
            {
                xmlOutputter.SaveToDisk(Path.Combine(options.OutputDirectory, options.SaveXmlPath));
            }
            return xmlOutputter;
        }

        private static void OutputHtml(Project project, XmlOutputter xmlOutputter, Options options)
        {
            var reader = new XElement("Source",
                (from file in project.SubObjectsOfType<File>()
                    select file.SourceXEle)).CreateReader();

            if (EnvVar.Verbose >= 2) Console.Write("Done" + Environment.NewLine + "Generating htmls... ");
            var htmlOutputter = new HtmlOutputter(EnvVar.XsltFullPathAndName(options.ThemeName));
            htmlOutputter.SetParameter("verbose", EnvVar.Verbose);
            htmlOutputter.SetParameter("source", reader);
            htmlOutputter.SaveToDisk(xmlOutputter.XDocument, options.OutputDirectory + EnvVar.slash);
        }

        private static void OutputTheme(Options options)
        {
            if (EnvVar.Verbose >= 2) Console.Write("Done" + Environment.NewLine + "Outputting theme files... ");
            var themeOutputter = new ThemeOutputter();
            themeOutputter.Output(options);
        }

        private static void PostProcessProject(ref Project project)
        {
            if (EnvVar.Verbose >= 2) Console.Write("Done" + Environment.NewLine + "Post processing project... ");
            foreach (var traverser in PluginManager.Traversers.ToList().OrderBy(t => t.Value.Key))
                traverser.Value.Value.Go(project);
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