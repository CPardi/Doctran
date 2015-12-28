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

    public enum ReportGenre
    {
        Licensing,
        Unlicensed,
        Argument,
        FileRead,
        Parsing,
        ParsingPost,
        Plugin,
        ProjectFile,
        XsltCompilation,
        XsltRuntime
    }

    public enum ReportSeverity
    {
        Undefined,

        Error,

        Warning,

        Message,

        Status
    }

    public class ConsolePublisher
    {
        private readonly Dictionary<ReportSeverity, string> _reportTitles = new Dictionary<ReportSeverity, string>
        {
            { ReportSeverity.Error, "Error" },
            { ReportSeverity.Warning, "Warning" },
            { ReportSeverity.Message, "Message" }
        };

        private readonly Dictionary<ReportGenre, string> _genreDescriptions = new Dictionary<ReportGenre, string>
        {
            { ReportGenre.Licensing, "A licensing issue has occured." },
            { ReportGenre.Unlicensed, "Unlicensed plugin in use." },
            { ReportGenre.Argument, "Argument list issue." },
            { ReportGenre.FileRead, "File read issue." },
            { ReportGenre.Parsing, "Parsing issue." },
            { ReportGenre.ParsingPost, "Parsing post-processing issue." },
            { ReportGenre.Plugin, "Plugin loading issue." },
            { ReportGenre.ProjectFile, "Project file contains an issue." },
            { ReportGenre.XsltCompilation, "Fatal XSLT stylesheet compilation issue." },
            { ReportGenre.XsltRuntime, "Fatal XSLT stylesheet runtime issue." },
        };

        private ReportGenre ReportGenre { get; set; }

        private string Location { get; set; }

        private string Reason { get; set; }

        public void DescriptionReasonLocation(ReportGenre reportGenre, string reason, string location)
        {
            AddReportGenre(reportGenre);
            AddReason(reason);
            AddLocation(location);
        }
        
        public void DescriptionReason(ReportGenre reportGenre, string reason)
        {
            AddReportGenre(reportGenre);
            AddReason(reason);
        }
        
        private void AddReportGenre(ReportGenre reportGenre)
        {            
            this.ReportGenre = reportGenre;
        }

        private void AddLocation(string location)
        {
            Debug.Assert(Location.IsNullOrEmpty(), "Cannot specify more than one error location.");

            this.Location = location;
        }

        private void AddReason(string reason)
        {
            Debug.Assert(Reason.IsNullOrEmpty(), "Cannot specify more than one error reason.");
            
            this.Reason = reason;
        }
     
        public void Publish(ReportSeverity reportSeverity)
        {
            // If we need a new line then add it.
            OtherUtils.ConsoleGotoNewLine();

            // Give errors or warnings in standard form.
            var ttb = new TitledTextBuilder();
            ttb.Append(_reportTitles[reportSeverity], _genreDescriptions[this.ReportGenre]);

            // If given, write reason.
            if (!Reason.IsNullOrEmpty())
            {
                ttb.Append("Reason", Reason);
            }

            // If given, write location.
            if (!Location.IsNullOrEmpty())
            {
                ttb.Append("Location", Location);
            }

            Console.Write($"\n{ttb}");
        }
    }
}