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
    using ParsingElements.FortranObjects;
    using Plugins;
    using Reporting;
    using Utilitys;
    using System.Linq;

    internal static class ProgramHelper
    {
        public static Project ParseProject(IEnumerable<string> sourceFiles, bool runInSerial)
        {
            var files = runInSerial ? sourceFiles : sourceFiles.AsParallel();

            var parsedFiles = files.Select(path =>
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

                        var source = OtherUtils.ReadAllText(path);
                        return language.Parse(path, source);
                    })
                    .ToList();

            return new Project(parsedFiles);
        }
    }
}