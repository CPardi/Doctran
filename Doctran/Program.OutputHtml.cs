// <copyright file="Program.OutputHtml.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Linq;
    using Helper;
    using Output.Html;
    using Reporting;
    using Utilitys;

    public partial class Program
    {
        private static void OutputHtml(XmlOutputter xmlOutputter, XDocument sourceDocument, Options options)
        {
            StageStopwatch.Restart();
            Report.NewStatus("Generating htmls... ");

            var htmlOutputter = new XsltRunner(Path.Combine(EnvVar.ExecPath, "themes", options.ThemeName, "main.xslt"));

            htmlOutputter.Run(
                xmlOutputter.XDocument,
                Path.GetFullPath(options.OutputDirectory) + EnvVar.Slash,
                new KeyValuePair<string, object>("verbose", Report.Verbose),
                new KeyValuePair<string, object>("source", sourceDocument.CreateReader()));

            SaveTiming("html-output", StageStopwatch.ElapsedMilliseconds);
            Report.ContinueStatus("Done");
        }
    }
}