﻿//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Doctran.Parsing.FortranObjects
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;
    using Helper;
    using MarkdownSharp;
    using Parsing;
    using Reporting;

    public class Description : XFortranObject
    {
        public string Basic { get; private set; }
        public string Detailed { get; private set; }

        private readonly Markdown _markdown = new Markdown();
        private readonly string _linkedTo;

        public string LinkedTo => _linkedTo.ToLower();

        public Description() { }

        public Description(string basicText)
            : base("Description", new List<FileLine> { new FileLine(-1, basicText) })
        {
            this.Basic = basicText;
        }

        public Description(string linkedTo, string basicText)
            : base("Description", new List<FileLine> { new FileLine(-1, basicText) })
        {
            this._linkedTo = linkedTo;
            this.Basic = basicText;
        }

        public Description(string linkedTo, string basicText, string detailedText, List<FileLine> lines)
            : base("Description", lines)
        {
            this._linkedTo = linkedTo;
            this.Basic = basicText;
            this.Detailed = detailedText;
        }

        public Description(List<FileLine> lines)
            : base("Description", lines)
        {
            this.Basic = string.Concat(
                from line in lines
                where Regex.IsMatch(line.Text, @"^\s*!>") && !Regex.IsMatch(line.Text, @"^\s*!>>")
                select (Regex.Match(line.Text, @"!>(.*)").Groups[1].Value.Trim()) + " ").TrimEnd();
            this.Detailed = MergeLines(lines);
        }

        public static string MergeLines(List<FileLine> lines)
        {
            return string.Concat(
                from line in lines
                where Regex.IsMatch(line.Text, @"^\s*!>>(.*)")
                select (Regex.Match(line.Text, @"!>>(.*)").Groups[1].Value) + "\n");
        }

        protected override string GetIdentifier()
        {
            return $"Description{this.Name}";
        }

        protected XElement Parse(string name, string text)
        {
            try
            {
                return XElement.Parse("<" + name + ">" + text + "</" + name + ">", LoadOptions.PreserveWhitespace);
            }
            catch
            {
                var curObj = this;
                var file = this.GoUpTillType<File>();

                Report.Warning(pub =>
                {
                    pub.AddWarningDescription("Description meta-data was ignored.");
                    pub.AddReason("Description could not be parsed.");
                    pub.AddLocation(curObj.lines.First().Number == curObj.lines.Last().Number
                        ? $"At line {curObj.lines.First().Number} of '{file.Name}{file.Info.Extension}'."
                        : $"Within lines {curObj.lines.First().Number} to {curObj.lines.Last().Number} of '{file.Name}{file.Info.Extension}'.");
                });

                return new XElement(name);
            }
        }

        public override XElement XEle()
        {
            var xele = new XElement(this.XElementName);

            xele.Add(this.Parse("Basic", WebUtility.HtmlEncode(this.Basic.Replace("\"", "\\" +  "\""))));

            if (string.IsNullOrEmpty(this.Detailed))
            {
                return xele;
            }

            xele.Add(this.Parse("Detailed", _markdown.Transform(this.Detailed)));
            return xele;
        }
    }

}