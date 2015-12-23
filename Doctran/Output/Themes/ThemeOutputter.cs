// <copyright file="ThemeOutputter.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Output.Themes
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using Helper;
    using Reporting;
    using Utilitys;

    public class ThemeOutputter
    {
        private readonly Dictionary<string, IFileCopier> _copiers = new Dictionary<string, IFileCopier>();

        private readonly DefaultCopier _defaultCopier = new DefaultCopier();

        public ThemeOutputter()
        {
            this.AddCopier(new LessFileCopier());
            this.AddCopier(new MarkdownFileCopier());
            this.AddCopier(new XsltFileIgnorer());
        }

        private void AddCopier(IFileCopier copier)
        {
            foreach (var ext in copier.FromExtensions)
            {
                _copiers.Add(ext, copier);
            }
        }

        public void Output(Options options)
        {
            var relativeDirectory = Path.GetDirectoryName(options.ProjectFilePath ?? EnvVar.DefaultInfoPath) + EnvVar.Slash;
            var outputDirectory = options.OutputDirectory + EnvVar.Slash;

            // Copy user's extra files.
            foreach (var filePath in options.CopyPaths)
            {
                this.CopyFile(options.OverwriteExisting, false, outputDirectory, relativeDirectory, relativeDirectory + filePath);
            }

            // Copy and parse user's extra files.
            foreach (var filePath in options.CopyAndParsePaths)
            {
                this.CopyFile(options.OverwriteExisting, true, outputDirectory, relativeDirectory, relativeDirectory + filePath);
            }

            // Copy theme files.
            this.CopyFilesFromDir(options.OverwriteExisting, true, true, outputDirectory, EnvVar.ThemeDirectory(options.ThemeName), "");
        }

        private void CopyFile(bool overwrite, bool parse, string outputDirectory, string relativeDirectory, string filePath)
        {
            var extension = Path.GetExtension(filePath) ?? string.Empty;

            IFileCopier copier;
            _copiers.TryGetValue(extension, out copier);

            var outputFilePath = PathUtils.ChangeExtension(Path.Combine(outputDirectory, PathUtils.PathRelativeTo(relativeDirectory, filePath)), copier?.ToExtension);

            if (!(copier is FileIgnorer))
            {
                Report.Message("Copying", $"'{Path.GetFullPath(filePath)}' to '{Path.GetFullPath(outputFilePath)}'");
            }

            if (copier == null || !parse)
            {
                _defaultCopier.Run(filePath, outputFilePath, overwrite);
            }
            else
            {
                copier.Run(filePath, outputFilePath, overwrite);
            }
        }
        
        private void CopyFilesFromDir(bool overwrite, bool parse, bool recursive, string outputDirectory, string relativeDirectory, string directoryPath)
        {
            var from = Path.Combine(relativeDirectory, directoryPath);
            Report.Message("Copying Directory", $"'{Path.GetFullPath(from)}' to '{Path.GetFullPath(outputDirectory)}'");
            foreach (var filePath in Directory.GetFiles(from, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
            {
                this.CopyFile(overwrite, parse, outputDirectory, relativeDirectory, filePath);
            }
        }
    }
}