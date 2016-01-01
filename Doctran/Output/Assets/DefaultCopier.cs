// <copyright file="DefaultCopier.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Output.Assets
{
    using System.IO;

    internal class DefaultCopier
    {
        public void Run(string filePath, string outputPath, bool overwrite)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(outputPath)));
            if (!File.Exists(outputPath) || overwrite)
            {
                File.Copy(filePath, outputPath, overwrite);
            }
        }
    }
}