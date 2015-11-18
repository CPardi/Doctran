//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Doctran.Parsing.BuiltIn.FortranObjects
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using Helper;

    public class NamedDescription : FortranObject
    {
        public string LinkedTo { get; }

        public NamedDescription(string linkedTo, XElement basic, XElement detailed, List<FileLine> lines)
            : base(lines)
        {
            this.LinkedTo = linkedTo;
            this.Basic = basic;
            this.Detailed = detailed;
        }

        public XElement Basic { get; }

        public XElement Detailed { get; }

        protected override string GetIdentifier() => "Linked Description";
    }

    public class Description2 : FortranObject
    {
        public Description2(XElement basic, XElement detailed, List<FileLine> lines)
            :base(lines)
        {
            this.Basic = basic;
            this.Detailed = detailed;
        }

        public XElement Basic { get; }

        public XElement Detailed { get; }

        protected override string GetIdentifier() => "Linked Description";
    }

    //public class Description : XFortranObject
    //{
    //    public Description(string basicText)
    //        : base("Description", new List<FileLine> { new FileLine(-1, basicText) })
    //    {
    //        this.Basic = basicText;
    //    }

    //    public Description(string basicText, string detailedText, List<FileLine> lines)
    //        : base("Description", lines)
    //    {
    //        this.Basic = basicText;
    //        this.Detailed = detailedText;
    //    }

    //    public string Basic { get; }
    //    public string Detailed { get; }

    //    //public string LinkedTo => _linkedTo?.ToLower();

    //    public static string MergeLines(List<FileLine> lines)
    //    {
    //        return string.Concat(
    //            from line in lines
    //            where Regex.IsMatch(line.Text, @"^\s*!>>(.*)")
    //            select (Regex.Match(line.Text, @"!>>(.*)").Groups[1].Value) + "\n");
    //    }

    //    public override XElement XEle()
    //    {
    //        var xele = new XElement(this.XElementName);

    //        xele.Add(this.Parse("Basic", WebUtility.HtmlEncode(this.Basic.Replace("\"", "\\" + "\""))));

    //        if (string.IsNullOrEmpty(this.Detailed))
    //        {
    //            return xele;
    //        }

    //        return xele;
    //    }

    //    protected override string GetIdentifier()
    //    {
    //        return $"Description{this.Name}";
    //    }

    //    protected XElement Parse(string name, string text)
    //    {
    //        try
    //        {
    //            return XElement.Parse("<" + name + ">" + text + "</" + name + ">", LoadOptions.PreserveWhitespace);
    //        }
    //        catch
    //        {
    //            var curObj = this;
    //            var file = this.GoUpTillType<SourceFile>();

    //            Report.Warning(pub =>
    //            {
    //                pub.AddWarningDescription("Description meta-data was ignored.");
    //                pub.AddReason("Description could not be parsed.");
    //                pub.AddLocation(curObj.lines.First().Number == curObj.lines.Last().Number
    //                    ? $"At line {curObj.lines.First().Number} of '{file.Name}{file.Extension}'."
    //                    : $"Within lines {curObj.lines.First().Number} to {curObj.lines.Last().Number} of '{file.Name}{file.Extension}'.");
    //            });

    //            return new XElement(name);
    //        }
    //    }
    //}
}