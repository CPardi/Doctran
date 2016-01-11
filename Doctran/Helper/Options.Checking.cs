// <copyright file="Options.Checking.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Helper
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CommandLine;
    using Reporting;
    using Utilitys;
    using Z.Reflection.Extensions;

    public partial class Options
    {
        private static OptionAttribute GetOptionAttribute(string propertyName)
        {
            var attr = (OptionAttribute)typeof(Options).GetProperty(propertyName).GetCustomAttribute(typeof(OptionAttribute));
            return attr;
        }

        private static void ReportError(Exception e, string propertyName)
        {
            var attr = GetOptionAttribute(propertyName);
            var shortNameStr = attr.ShortName == null ? string.Empty : $"(-{attr.ShortName})";
            Report.Error(p => p.DescriptionReason(ReportGenre.Argument, $"Value for flag --{attr.LongName}{shortNameStr} contains an error. {e.Message}"), e);
        }

        private string CheckCommandPathExists(string propertyName, string path)
        {
            if (File.Exists(path))
            {
                return path;
            }

            var fullPath = Path.GetFullPath(path);
            ReportError(new FileNotFoundException($"Could not find file at '{fullPath}'", fullPath), propertyName);
            return path;
        }

        private string CheckCommandLinePath(string propertyName, string path)
        {
            try
            {
                Path.GetFullPath(path);
            }
            catch (Exception e)
            {
                ReportError(e, propertyName);
            }

            return path;
        }

        private List<string> CheckPathList(ReportGenre genre, IEnumerable<string> list)
        {
            var pathList = new PathList { PathStorage = PathList.PathStorageMode.Absolute };
            try
            {
                pathList.AddRange(list);
            }
            catch (Exception e)
            {
                Report.Error(p => p.DescriptionReasonLocation(genre, $"Error in path to source file. {e.Message}", this.ProjectFilePath), e);
            }

            return pathList.ToList();
        }

        private string CheckThemeExists(string propertyName, string themeName)
        {
            if (!Directory.Exists(Path.Combine(EnvVar.ThemeDirectory(themeName))))
            {
                ReportError(new NotImplementedException($"A theme named '{themeName}' does not exist."), propertyName);
            }

            return themeName;
        }

        private int CheckVerboseRange(string propertyName, int value)
        {
            if (value < 1 || value > 3)
            {
                ReportError(new IndexOutOfRangeException("Verbosity must be a value between 1 and 3."), propertyName);
            }

            return value;
        }
    }
}