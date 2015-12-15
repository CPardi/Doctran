// <copyright file="ProgramHelper.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Helper
{
    using System.Collections.Generic;
    using System.IO;
    using Parsing;
    using Parsing.BuiltIn.FortranObjects;
    using Plugins;
    using Reporting;

    internal static class ProgramHelper
    {
        public static Project ParseProject(IEnumerable<string> sourceFiles)
        {
            var parsedFiles = new List<SourceFile>();

            foreach (var path in sourceFiles)
            {
                if (!File.Exists(path))
                {
                    var e = new FileNotFoundException();
                    Report.Error((pub, ex) => { pub.AddErrorDescription("Source file path does not exist."); }, e);
                }

                // Parse source files.
                var language = PluginLoader.GetLanguageFromExtension(path);
                var parsedFile = new Parser(language.BlocksParsers).ParseFile(path, SourceFile.ReadFile(path));
                foreach (var t in language.Traversers)
                {
                    t.Go(parsedFile);
                }

                parsedFiles.Add(parsedFile);
            }

            return new Project(parsedFiles);
        }
    }
}