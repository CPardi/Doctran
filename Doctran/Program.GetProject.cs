﻿namespace Doctran
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

    public partial class Program
    {
        private static Project GetProject(IEnumerable<string> sourceFiles, bool runInSerial)
        {
            StageStopwatch.Restart();
            Report.NewStatus("Analysing project block structure... ");

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
            }).ToList();

            var project = new Project(parsedFiles, languageList.Select(lang => lang.GlobalScopeFactory));
            new Traverser("Global post processing", languageList.SelectMany(p => p.GlobalTraverserActions).ToArray()).Go(project);

            SaveTiming("project-parsing", StageStopwatch.ElapsedMilliseconds);
            Report.ContinueStatus("Done");
            return project;
        }
    }
}