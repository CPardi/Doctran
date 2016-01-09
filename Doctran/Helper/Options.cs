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
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using CommandLine;
    using CommandLine.Text;
    using Input.Options;
    using Input.ProjectFileOptions;
    using Reporting;
    using Utilitys;

    public partial class Options
    {
        /// <summary>
        ///     Gets or sets a list of paths to be parsed and copied to the output directory. Paths are stored relative to the
        ///     project file.
        /// </summary>
        [Input.Options.OptionList("CopyAndParse", typeof(PathList))]
        public PathList CopyAndParsePaths { get; set; } = new PathList { PathStorage = PathList.PathStorageMode.Relative };

        /// <summary>
        ///     Gets or sets a list of paths to be copied to the output directory. Paths are stored relative to the project file.
        /// </summary>
        [Input.Options.OptionList("Copy", typeof(PathList))]
        public PathList CopyPaths { get; set; } = new PathList { PathStorage = PathList.PathStorageMode.Relative };

        /// <summary>
        ///     Gets or sets a path to save the documentation.
        /// </summary>
        [CommandLine.Option('o', "output", DefaultValue = "Docs", MetaValue = "PATH", HelpText = "PATH is the output directory for the documentation.")]
        public string OutputDirectory
        {
            get { return _outputDirectory; }
            set { _outputDirectory = this.CheckCommandLinePath(nameof(this.OutputDirectory), value); }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether an existing output should be overwritten.
        /// </summary>
        [CommandLine.Option("overwrite", HelpText = "If this option is specified, then the auxiliary files (css, scripts, etc.) and html files in the output directory will be overwritten. If not, then just the html files will be overwritten.")]
        public bool OverwriteExisting { get; set; }

        /// <summary>
        ///     Gets or sets the location of a project file.
        /// </summary>
        [CommandLine.Option('p', "project_info", MetaValue = "PATH", HelpText = "PATH is the location of the project's information file.")]
        public string ProjectFilePath
        {
            get { return _projectFilePath; }
            set { _projectFilePath = this.CheckCommandLinePath(nameof(this.ProjectFilePath), value); }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the generated XML files should be saved within the output directory.
        /// </summary>
        [CommandLine.Option("save_xmls", MetaValue = "PATH", HelpText = "Specifies a path to save the intermediary XML document generated. The document is not saved by default.")]
        public bool SaveXmls { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether information about licensing should be shown to the user.
        /// </summary>
        [CommandLine.Option("license", DefaultValue = false, HelpText = "Run Doctran with this option to enter a license key and view license information.")]
        public bool ShowLicensing { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether  information about installed plugins should be shown to the user.
        /// </summary>
        [CommandLine.Option("plugins", HelpText = "This option forces Doctran to check the plugins folder for any shared libraries present and write their names and version numbers.")]
        public bool ShowPluginInformation { get; set; }

        /// <summary>
        ///     Gets the paths of the source files specified by the user from the command line and the project file.
        /// </summary>
        public List<string> SourceFilePaths => this.CheckPathList(ReportGenre.Argument, this.SourceFilesFromCommandLine)
            .Concat(this.SourceFilesFromOptionFile)
            .ToList();

        /// <summary>
        ///     Sets the paths of the source files specified by the user from the command line.
        /// </summary>
        /// <remarks>The is only public in order to be found when using reflection.</remarks>
        [ValueList(typeof(List<string>))]
        public List<string> SourceFilesFromCommandLine { private get; set; } = new List<string>();

        /// <summary>
        ///     Sets the paths of the source files specified by the user from the project file.
        /// </summary>
        /// <remarks>The is only public in order to be found when using reflection.</remarks>
        [Input.Options.OptionList("Source", typeof(PathList), ListMode = ListMode.AddTo)]
        public PathList SourceFilesFromOptionFile { private get; set; } = new PathList();

        /// <summary>
        ///     Gets or sets the name of the theme that should be used.
        /// </summary>
        [CommandLine.Option('t', "theme", DefaultValue = "Default", MetaValue = "NAME", HelpText = "NAME is the name of the theme to be applied.")]
        public string ThemeName
        {
            get { return _themeName; }
            set { _themeName = this.CheckThemeExists(nameof(this.ThemeName), value); }
        }

        /// <summary>
        ///     Gets or sets a value specifying how much output should be generated.
        /// </summary>
        [CommandLine.Option("verbose", DefaultValue = 2, MetaValue = "VALUE", HelpText = "Set how much output is generated.")]
        public int Verbose
        {
            get { return _verbose; }
            set { _verbose = this.CheckVerboseRange(nameof(this.Verbose), value); }
        }

        /// <summary>
        ///     Gets or sets the additional XML elements to be appended to the project's XML file.
        /// </summary>
        [XmlDefaultOption(InitializeAsDefault = true)]
        [MenuOption]
        [UserPageOption]
        public List<XElement> XmlInformation { get; set; }

        [HelpOption]
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