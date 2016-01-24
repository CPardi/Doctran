// <copyright file="ConsolePublisher.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Reporting
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Helper;
    using Utilitys;

    public class ConsolePublisher
    {
        private readonly Dictionary<ReportGenre, string> _genreDescriptions = new Dictionary<ReportGenre, string>
        {
            { ReportGenre.Licensing, "A licensing issue has occured." },
            { ReportGenre.Unlicensed, "Unlicensed plugin in use." },
            { ReportGenre.Argument, "Argument list issue." },
            { ReportGenre.FileRead, "File read issue." },
            { ReportGenre.Parsing, "Parsing issue." },
            { ReportGenre.Traversal, "Project traversal issue." },
            { ReportGenre.Plugin, "Plugin loading issue." },
            { ReportGenre.ProjectFile, "Project file contains an issue." },
            { ReportGenre.ThemeOutput, "Problem outputting documentation." },
            { ReportGenre.XsltCompilation, "Fatal XSLT stylesheet compilation issue." },
            { ReportGenre.XsltRuntime, "Fatal XSLT stylesheet runtime issue." }
        };

        private readonly Dictionary<ReportSeverity, string> _reportStreams = new Dictionary<ReportSeverity, string>
        {
            { ReportSeverity.Error, "Error" },
            { ReportSeverity.Warning, "Warning" },
            { ReportSeverity.Message, "Message" }
        };

        private readonly Dictionary<ReportSeverity, string> _reportTitles = new Dictionary<ReportSeverity, string>
        {
            { ReportSeverity.Error, "Error" },
            { ReportSeverity.Warning, "Warning" },
            { ReportSeverity.Message, "Message" }
        };

        private string Location { get; set; }

        private string Reason { get; set; }

        private ReportGenre ReportGenre { get; set; }

        public void DescriptionReason(ReportGenre reportGenre, string reason)
        {
            this.AddReportGenre(reportGenre);
            this.AddReason(reason);
        }

        public void DescriptionReasonLocation(ReportGenre reportGenre, string reason, string location)
        {
            this.AddReportGenre(reportGenre);
            this.AddReason(reason);
            this.AddLocation(location);
        }

        public void Publish(ReportSeverity reportSeverity)
        {
            // If we need a new line then add it.
            OtherUtils.ConsoleGotoNewLine();

            // Give errors or warnings in standard form.
            var ttb = new TitledTextBuilder();
            ttb.Append(_reportTitles[reportSeverity], _genreDescriptions[this.ReportGenre]);

            // If given, write reason.
            if (!this.Reason.IsNullOrEmpty())
            {
                ttb.Append("Reason", this.Reason);
            }

            // If given, write location.
            if (!this.Location.IsNullOrEmpty())
            {
                ttb.Append("Location", this.Location);
            }

            if (reportSeverity == ReportSeverity.Error)
            {
                Console.Error.WriteLine($"\n{ttb}");
            }
            else
            {
                Console.Write($"\n{ttb}");
            }
        }

        private void AddLocation(string location)
        {
            Debug.Assert(this.Location.IsNullOrEmpty(), "Cannot specify more than one error location.");

            this.Location = location;
        }

        private void AddReason(string reason)
        {
            Debug.Assert(this.Reason.IsNullOrEmpty(), "Cannot specify more than one error reason.");

            this.Reason = reason;
        }

        private void AddReportGenre(ReportGenre reportGenre)
        {
            this.ReportGenre = reportGenre;
        }
    }
}