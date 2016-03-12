// <copyright file="AssetOutputter.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Output.Assets
{
    using System.Collections.Generic;
    using System.IO;
    using Helper;
    using Html;
    using Reporting;
    using Utilitys;

    public class AssetOutputter
    {
        private readonly IEnumerable<string> _themeParts;

        private readonly Dictionary<string, IFileCopier> _copiers = new Dictionary<string, IFileCopier>();

        private readonly DefaultCopier _defaultCopier = new DefaultCopier();

        public AssetOutputter(IEnumerable<string> themeParts)
        {
            _themeParts = themeParts;
            this.AddCopier(new LessFileCopier());
            this.AddCopier(new MarkdownFileCopier());
            this.AddCopier(new XsltFileIgnorer());
        }

        public void Output(bool overwriteExisting, string outputDirectory, string projectFilePath, string themeName, IEnumerable<string> copyPaths, IEnumerable<string> copyAndParsePaths)
        {
            var relativeDirectory = Path.GetDirectoryName(projectFilePath ?? EnvVar.DefaultInfoPath) ?? string.Empty;

            // Copy user's extra files.
            foreach (var filePath in copyPaths)
            {
                this.CopyFile(overwriteExisting, false, outputDirectory, relativeDirectory, Path.Combine(relativeDirectory, filePath));
            }

            // Copy and parse user's extra files.
            foreach (var filePath in copyAndParsePaths)
            {
                this.CopyFile(overwriteExisting, true, outputDirectory, relativeDirectory, Path.Combine(relativeDirectory, filePath));
            }

            // Copy the parts needed from theme files.
            foreach (var part in _themeParts)
            {
                this.CopyFilesFromDir(overwriteExisting, true, true, outputDirectory, EnvVar.ThemeOutputPath(themeName), part);
            }
        }

        private void AddCopier(IFileCopier copier)
        {
            foreach (var ext in copier.FromExtensions)
            {
                _copiers.Add(ext, copier);
            }
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