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
    using System.Linq;
    using Parsing;
    using ParsingElements.FortranObjects;
    using Plugins;
    using Reporting;
    using Utilitys;
    using Z.Collections.Extensions;

    internal static class ProgramHelper
    {
        public static Project ParseProject(IEnumerable<string> sourceFiles, bool runInSerial)
        {
            var files = runInSerial ? sourceFiles : sourceFiles.AsParallel();
            var languageList = new List<ILanguageParser>();

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

                languageList.AddIf(p => languageList.Select(p2 => p2.Identifier).All(i => i != p.Identifier), language);
                var source = OtherUtils.ReadAllText(path);
                return language.Parse(path, source);
            })
                .ToList();

            var project = new Project(parsedFiles);
            new Traverser("Global post processing", languageList.SelectMany(p => p.GlobalTraverserActions).ToArray()).Go(project);
            return project;
        }
    }
}