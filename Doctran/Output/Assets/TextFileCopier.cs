// <copyright file="TextFileCopier.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Output.Assets
{
    using System.Collections.ObjectModel;
    using System.IO;

    internal abstract class TextFileCopier : IFileCopier
    {
        public abstract ReadOnlyCollection<string> FromExtensions { get; }

        public abstract string ToExtension { get; }

        public void Run(string filePath, string outputPath, bool overwrite)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
            if (overwrite || !File.Exists(outputPath))
            {
                File.WriteAllText(outputPath, this.ReadFile(filePath));
            }
        }

        protected virtual string ReadFile(string filePath)
        {
            using (var fileReader = new StreamReader(filePath))
            {
                return fileReader.ReadToEnd();
            }
        }
    }
}