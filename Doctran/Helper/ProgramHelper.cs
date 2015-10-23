namespace Doctran.Helper
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Parsing;
    using Parsing.FortranObjects;
    using Reporting;
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
                var ext = Path.GetExtension(path);
                ILanguage language;
                if (!LanguageManager.TryGetLanguage(ext, out language))
                {
                    var e = new ApplicationException($"'{path}' could not be parsed as no language is registered to the file extension '{ext}'.");
                    Report.Error((pub, ex) => { pub.AddErrorDescription(ex.Message); }, e);
                    throw e;
                }

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