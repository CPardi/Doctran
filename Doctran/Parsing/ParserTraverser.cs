// <copyright file="ParserTraverser.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Helper;
    using ParsingElements;
    using Reporting;
    using Utilitys;

    public class ParserTraverser
    {
        public ParserTraverser(string language, Func<string, IEnumerable<FileLine>> preprocessor, IEnumerable<IFortranBlock> blockParsers, IEnumerable<Traverser> traversers)
        {
            this.Language = language;
            this.Preprocessor = preprocessor;
            this.BlockParsers = blockParsers;
            this.Traversers = traversers;
        }

        public IEnumerable<IFortranBlock> BlockParsers { get; }

        public IErrorListener<ParserException> ErrorListener { get; set; } = new StandardErrorListener<ParserException>();

        public string Language { get; }

        public Func<string, IEnumerable<FileLine>> Preprocessor { get; set; }

        public IEnumerable<Traverser> Traversers { get; }

        public ISourceFile Parse(string sourceName, string source)
        {
            // Parse and post process AST.
            var pListener = new ListenerAndAggregater<ParserException>();
            var sourceFile = new Parser(this.Language, this.BlockParsers) { ErrorListener = pListener }
                .Parse(sourceName, source, this.Preprocessor);

            // Report warnings for both warnings and errors frot he parser.
            var pErrors = pListener.Warnings.Concat(pListener.Errors).ToList();
            if (pErrors.Any())
            {
                Report.Warnings((pub, e) => CreatePActionPublisher(sourceName, pub, e), pErrors.OrderBy(err => err.StartLine));
            }

            var tListener = new ListenerAndAggregater<TraverserException>();
            foreach (var t in this.Traversers)
            {
                t.ErrorListener = tListener;
                t.Go(sourceFile);
            }

            // Report warnings for both warnings and errors from the traversers.
            var tErrors = tListener.Warnings.Concat(tListener.Errors).ToList();
            if (tErrors.Any())
            {
                Report.Warnings((pub, e) => CreateTActionPublisher(sourceName, e, pub), tErrors.OrderBy(err => (err.Cause as IHasLines)?.Lines.FirstOrDefault().Number));
            }

            return sourceFile;
        }

        private static void CreatePActionPublisher(string sourceName, ConsolePublisher pub, ParserException e)
        {
            pub.DescriptionReasonLocation(ReportGenre.Parsing, $"{e.Message} Output may contain unexpected results.", StringUtils.LocationString(e.StartLine, e.EndLine, sourceName));
        }

        private static void CreateTActionPublisher(string sourceName, TraverserException e, ConsolePublisher pub)
        {
            var lines = (e.Cause as IHasLines)?.Lines;
            if (lines != null)
            {
                pub.DescriptionReasonLocation(
                    ReportGenre.Traversal,
                    $"{e.Message} Output may contain unexpected results.",
                    StringUtils.LocationString(lines.First().Number, lines.Last().Number, sourceName));
            }
            else
            {
                pub.DescriptionReason(ReportGenre.Traversal, $"{e.Message} Output may contain unexpected results.");
            }
        }
    }
}