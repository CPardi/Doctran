// <copyright file="ProgramHelper.cs" company="Christopher Pardi">
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
    using ParsingElements;
    using ParsingElements.FortranObjects;
    using Plugins;
    using Reporting;
    using Utilitys;

    internal static class ProgramHelper
    {
        public static Project ParseProject(IEnumerable<string> sourceFiles)
        {
            var parsedFiles = new List<ISourceFile>();

            foreach (var path in sourceFiles)
            {
                Report.Message("Parsing", path);

                // Parse source files.
                ILanguageParser language;
                try
                {
                    language = ParserManager.GetParserByExtension(Path.GetExtension(path));
                }
                catch (NotSupportedException e)
                {
                    Report.Error(p => p.DescriptionReasonLocation(ReportGenre.FileRead, e.Message, path), e);
                    return null;
                }

                var lines = OtherUtils.ReadFile(path);
                var parsedFile = language.Parse(path, lines);
                parsedFiles.Add(parsedFile);
            }

            return new Project(parsedFiles);
        }
    }
}