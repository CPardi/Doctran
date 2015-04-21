//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Linq;
using System.IO;

namespace Doctran.Fbase.Outputters
{
    public class ThemeOutputter
    {
        private readonly bool overwriteTheme;

        public ThemeOutputter(bool overwriteTheme)
        {
            this.overwriteTheme = overwriteTheme;
        }

        public void Output(String themePath, String outputDirectory)
        {
            String[] allFiles = Directory.GetFiles(themePath, "*", SearchOption.AllDirectories);
            String[] excludedFiles = Directory.GetFiles(themePath, "*.xslt", SearchOption.AllDirectories);
            String[] filesToCopy = allFiles.Except(excludedFiles).ToArray();

            foreach (String filePath in filesToCopy)
            {
                String outputPath = outputDirectory + filePath.Substring(themePath.Length);
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
                if (!File.Exists(outputPath) | overwriteTheme)
                {
                    File.Copy(filePath, outputPath, overwriteTheme);
                }
            }
        }
    }
}
