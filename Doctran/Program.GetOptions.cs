// <copyright file="Program.GetOptions.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran
{
    using System;
    using System.IO;
    using System.Linq;
    using Helper;
    using Input.Options;
    using Reporting;
    using Utilitys;

    public partial class Program
    {
        private static void GetOptions(string path, Options options)
        {
            var parserExceptions = new ListenerAndAggregater<OptionReaderException>();
            var projectFileReader = new OptionsReader<Options>(5, "Project file") { ErrorListener = parserExceptions };
            var projFileSource = OtherUtils.ReadAllText(path);

            PathUtils.RunInDirectory(
                Path.GetDirectoryName(path),
                () => projectFileReader.Parse(options, path, projFileSource));
            var ws = parserExceptions.Warnings;
            var es = parserExceptions.Errors;

            // Report any errors to the user.
            Action<ConsolePublisher, OptionReaderException> action
                = (p, e) => p.DescriptionReasonLocation(ReportGenre.ProjectFile, e.Message, StringUtils.LocationString(e.StartLine, e.EndLine, Path.GetFullPath(path)));

            if (ws.Any())
            {
                Report.Warnings(action, ws);
            }

            if (es.Any())
            {
                Report.Errors(action, es);
            }
        }
    }
}