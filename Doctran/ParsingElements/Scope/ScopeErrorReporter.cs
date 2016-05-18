// <copyright file="ScopeErrorReporter.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.ParsingElements.Scope
{
    using System.Collections.Generic;
    using System.Linq;
    using Helper;
    using Parsing;
    using Reporting;
    using Utilitys;

    public class ScopeErrorReporter : IErrorListener<TraverserException>
    {
        public void Error(TraverserException exception)
        {
            var lines = GetLines(exception);
            var firstLine = lines?.First().Number ?? -1;
            var lastLine = lines?.Last().Number ?? -1;
            var filePath = GetFilePath(exception);
            Report.Warning(
                p => { p.DescriptionReasonLocation(ReportGenre.Traversal, exception.Message, StringUtils.LocationString(firstLine, lastLine, filePath)); });
        }

        public void Warning(TraverserException exception)
        {
            var lines = GetLines(exception);
            var firstLine = lines?.First().Number ?? -1;
            var lastLine = lines?.Last().Number ?? -1;
            var filePath = GetFilePath(exception);
            Report.Warning(
                p => { p.DescriptionReasonLocation(ReportGenre.Traversal, exception.Message, StringUtils.LocationString(firstLine, lastLine, filePath)); });
        }

        private static string GetFilePath(TraverserException exception)
        {
            return (exception.Cause as IFortranObject).SelfOrAncestorOfType<ISourceFile>()?.AbsolutePath;
        }

        private static List<FileLine> GetLines(TraverserException exception)
        {
            return (exception.Cause as IHasLines)?.Lines ?? ((exception.Cause as IContained)?.Parent as IHasLines)?.Lines;
        }
    }
}