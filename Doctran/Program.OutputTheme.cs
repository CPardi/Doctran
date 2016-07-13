// <copyright file="Program.OutputTheme.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran
{
    using System.IO;
    using System.Linq;
    using Helper;
    using Output.Assets;
    using Plugins;
    using Reporting;

    public partial class Program
    {
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
    }
}