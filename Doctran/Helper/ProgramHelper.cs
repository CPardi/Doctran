﻿namespace Doctran.Helper
{
    using System.Collections.Generic;
    using System.IO;
    using Parsing;
    using Parsing.FortranObjects;
    using Reporting;
    using Utilitys;
    using File = Parsing.FortranObjects.File;

    internal static class ProgramHelper
    {
        public static Project2 ParseProject(IEnumerable<string> sourceFiles)
        {
            var parsedFiles = new List<File>();

            foreach (var path in sourceFiles)
            {
                if (!System.IO.File.Exists(path))
                {
                    var e = new FileNotFoundException();
                    Report.Error((pub, ex) => { pub.AddErrorDescription("Source file path does not exist."); }, e);
                    throw e;
                }

                // Parse source files.
                var language = PluginLoader.GetLanguageFromExtension(path);

                var parsedFile = new Parser(language.BlocksParsers).ParseFile(path, File.ReadFile(path), language.ObjectGroups);

                foreach (var t in language.Traversers)
                {
                    t.Go(parsedFile);
                }

                parsedFiles.Add(parsedFile);
            }

            return new Project2(parsedFiles);
        }
    }
}