//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Linq;
using System.IO;

using dotless.Core;

namespace Doctran.Fbase.Outputters
{
    public class ThemeOutputter
    {
        private readonly bool overwriteTheme;

        public ThemeOutputter(bool overwriteTheme)
        {
            this.overwriteTheme = overwriteTheme;
        }

        public void Output(String themePath, String colorScheme, String outputDirectory)
        {
            String[] allFiles = Directory.GetFiles(themePath, "*", SearchOption.AllDirectories);

            var xsltFiles = Directory.GetFiles(themePath, "*.xslt", SearchOption.AllDirectories);
            var lessFiles = Directory.GetFiles(themePath, "*.less", SearchOption.AllDirectories);

            foreach (var lessPath in lessFiles)
            {
                String relPath = outputDirectory + lessPath.Substring(themePath.Length);
                String outputPath = relPath.Remove(relPath.Length - 5) + ".css";
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

                String lessString;
                using (StreamReader FileReader = new StreamReader(lessPath)) { lessString = FileReader.ReadToEnd(); }

                try
                {
                    String curDir = Environment.CurrentDirectory;
                    Directory.SetCurrentDirectory(Common.Settings.execPath);
                    String cssText = Less.Parse( @"@import ""colorSchemes/" + colorScheme + @".less""; " + Environment.NewLine + lessString);
                    File.WriteAllText(outputPath, cssText);
                    Directory.SetCurrentDirectory(curDir);
                }
                catch(FileNotFoundException e)
                {
                    UserInformer.GiveError("LESS stylesheet", e.FileName, e);
                }
            }

            String[] filesToCopy = allFiles.Except(xsltFiles).Except(lessFiles).ToArray();

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
