// <copyright file="Options.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Xml.Linq;
    using CommandLine;
    using CommandLine.Text;
    using Input.ProjectFileOptions;
    using Utilitys;

    public class Options
    {
        [Input.Options.OptionList("CopyAndParse", typeof(PathList))]
        public PathList CopyAndParsePaths { get; set; } = new PathList();

        [Input.Options.OptionList("Copy", typeof(PathList))]
        public PathList CopyPaths { get; set; } = new PathList();

        [Option('o', "output", DefaultValue = "Docs", MetaValue = "PATH", HelpText = "PATH is the output directory for the documentation.")]
        public string OutputDirectory { get; set; }

        [Option("overwrite", HelpText = "If this option is specified, then the auxiliary files (css, scripts, etc.) and html files in the output directory will be overwritten. If not, then just the html files will be overwritten.")]
        public bool OverwriteExisting { get; set; }

        [Option('p', "project_info", MetaValue = "PATH", HelpText = "PATH is the location of the project's information file.")]
        public string ProjectFilePath { get; set; }

        [Option("save_xml", MetaValue = "PATH", HelpText = "Specifies a path to save the intermediary XML document generated. The document is not saved by default.")]
        public string SaveXmlPath { get; set; }

        [Option("help", HelpText = "Show this help and exit.")]
        public bool ShowHelp { get; set; }

        [Option("license", DefaultValue = false, HelpText = "Run Doctran with this option to enter a license key and view license information.")]
        public bool ShowLicensing { get; set; }

        [Option("plugins", HelpText = "This option forces Doctran to check the plugins folder for any shared libraries present and write their names and version numbers.")]
        public bool ShowPluginInformation { get; set; }

        [ValueList(typeof(PathList))]
        [Input.Options.OptionList("Source", typeof(PathList))]
        public PathList SourceFilePaths { get; set; }

        [Option('t', "theme", DefaultValue = "Default", MetaValue = "NAME", HelpText = "NAME is the name of the theme to be applied.")]
        public string ThemeName { get; set; }

        [Option("verbose", DefaultValue = 2, MetaValue = "VALUE", HelpText = "Set how much output is generated.")]
        public int Verbose { get; set; }

        /// <summary>
        ///     Additional XML elements to be appended to the project's XML file.
        /// </summary>
        [XmlDefaultOption(InitializeAsDefault = true)]
        [MenuOption]
        [UserPageOption]
        public List<XElement> XmlInformation { get; set; }

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
    }
}