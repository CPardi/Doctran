//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Reflection;
using CommandLine.Text;
using System.Xml.Linq;

namespace Doctran.Fbase.Common
{
    using System.IO;

    public class Options
    {
        public string GetUsage()
        {
            var assemblyTitle = this.GetType().Assembly.GetAssemblyAttribute<AssemblyTitleAttribute>().Title;
            var assemblyVersion = this.GetType().Assembly.GetName().Version.ToString();
            var assemblyCompany = this.GetType().Assembly.GetAssemblyAttribute<AssemblyCompanyAttribute>().Company;

            var help = new HelpText
            {
                Heading = new HeadingInfo(assemblyTitle, assemblyVersion),
                Copyright = new CopyrightInfo(assemblyCompany, 2015),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };

            help.AddPreOptionsLine(Environment.NewLine + "Usage: doctran [options] [source_files]");
            help.AddOptions(this);
            return help.ToString();
        }

        [CommandLine.Option("save_xml", MetaValue = "PATH", HelpText = "Specifies a path to save the intermediary XML document generated. The document is not saved by default.")]
        public string SaveXmlPath { get; set; }

        [CommandLine.Option('o', "output", DefaultValue = "Docs", MetaValue = "PATH", HelpText = "PATH is the output directory for the documentation.")]
        public string OutputDirectory { get; set; }

        [CommandLine.Option('p', "project_info", MetaValue = "PATH", HelpText = "PATH is the location of the project's information file.")]
        public string ProjectFilePath { get; set; }

        [CommandLine.Option('t', "theme", DefaultValue = "Default", MetaValue = "NAME", HelpText = "NAME is the name of the theme to be applied.")]
        public string ThemeName { get; set; }

        [CommandLine.Option("verbose", DefaultValue = 2, MetaValue = "VALUE", HelpText = "Set how much output is generated.")]
        public int Verbose{ get; set; }

        [CommandLine.Option("overwrite", HelpText = "If this option is specified, then the auxiliary files (css, scripts, etc.) and html files in the output directory will be overwritten. If not, then just the html files will be overwritten.")]
        public bool OverwriteExisting { get; set; }

        [CommandLine.Option("plugins", HelpText = "This option forces Doctran to check the plugins folder for any shared libraries present and write their names and version numbers.")]
        public bool ShowPluginInformation { get; set; }

        [CommandLine.Option("help", HelpText = "Show this help and exit.")]
        public bool ShowHelp { get; set; }

        [CommandLine.Option("license", DefaultValue = false, HelpText = "Run Doctran with this options to enter a license key and view license information.")]
        public bool ShowLicensing { get; set; }

        [CommandLine.ValueList(typeof(PathList))]
        [OptionFile.Value("Source")]
        public PathList SourceFilePaths { get; set; }

        [OptionFile.Value("Copy")]
        public PathList CopyPaths { get; set; } = new PathList(false);

        [OptionFile.Value("CopyAndParse")]
        public PathList CopyAndParsePaths { get; set; } = new PathList(false);

        [OptionFile.XmlPassThroughOptions("Information")]
        public XElement XmlInformation { get; set; } = new XElement("Information");
    }
}