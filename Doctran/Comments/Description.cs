//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Net;

using Doctran.Fbase.Projects;
using Doctran.Fbase.Files;
using Doctran.Fbase.Common;
using Doctran.Fbase.Comments;

using Doctran.BaseClasses;

using MarkdownSharp;

namespace Doctran.Exceptions
{
    using Reporting;

    public class DescriptionException : PostAction
    {
        public DescriptionException()
            : base(typeof(Description))
        { }

        public override void PostObject(ref FortranObject obj)
        {
            this.CorrectName(ref obj);
            this.CheckUniqueness(ref obj);
        }

        public void CorrectName(ref FortranObject obj)
        {
            if (obj.parent.Identifier == obj.Identifier)
            {
                return;
            }

            var curObj = obj;
            var file = obj.GoUpTillType<File>();
            Report.Warning(pub =>
            {
                pub.AddWarningDescription("Description meta-data was ignored");
                pub.AddReason("Description identifier does not match parent identifier.");
                pub.AddLocation(curObj.lines.First().Number == curObj.lines.Last().Number
                    ? $"At line {curObj.lines.First().Number} of '{file.Name}{file.Info.Extension}'."
                    : $"Within lines {curObj.lines.First().Number} to {curObj.lines.Last().Number} of '{file.Name}{file.Info.Extension}'.");
            });
            obj.parent.SubObjects.Remove(obj);
        }

        public void CheckUniqueness(ref FortranObject obj)
        {
            if (obj.parent.SubObjectsOfType<Description>().Count <= 1)
            {
                return;
            }

            var curObj = obj;
            var file = obj.GoUpTillType<File>();
            if (obj.parent is Project)
            {
                Report.Warning(pub =>
                {
                    pub.AddWarningDescription("Description meta-data was ignored");
                    pub.AddReason("Multiple descriptions specified for a single block.");
                    pub.AddLocation(curObj.lines.First().Number == curObj.lines.Last().Number
                        ? $"At line {curObj.lines.First().Number} of '{file.Name}{file.Info.Extension}'."
                        : $"Within lines {curObj.lines.First().Number} to {curObj.lines.Last().Number} of '{file.Name}{file.Info.Extension}'.");
                });
            }
            else
            {
                Report.Warning(pub =>
                {
                    pub.AddWarningDescription("Description meta-data was ignored");
                    pub.AddReason("Multiple descriptions specified for a single block.");
                    pub.AddLocation(curObj.lines.First().Number == curObj.lines.Last().Number
                        ? $"At line {curObj.lines.First().Number} of '{file.Name}{file.Info.Extension}'."
                        : $"Within lines {curObj.lines.First().Number} to {curObj.lines.Last().Number} of '{file.Name}{file.Info.Extension}'.");
                });
            }
            obj.parent.SubObjects.Remove(obj);
        }
    }
}

namespace Doctran.Fbase.Comments
{
    using Reporting;

    public class DescriptionBlock : FortranBlock
    {
        public DescriptionBlock()
            : base("Description", false, false, 3) { }

        public static string BlockName => "Description";

        public override bool BlockStart(string parentBlockName, List<FileLine> lines, int lineIndex)
        {
            if (parentBlockName == DescriptionBlock.BlockName) return false;
            return
                CommentDefinitions.DescStart(lines[lineIndex].Text)
                && !parentBlockName.StartsWith("Information_")
                && !CommentDefinitions.DetailLine(lines[lineIndex].Text)
                && !CommentDefinitions.NDescStart(lines[lineIndex].Text)
                && !CommentDefinitions.InfoStart(lines[lineIndex].Text);
        }

        public override bool BlockEnd(string parentBlockName, List<FileLine> lines, int lineIndex)
        {
            if (lines.Count == lineIndex + 1) return true;
            return
                CommentDefinitions.DescEnd(lines[lineIndex + 1].Text)
                || CommentDefinitions.InfoStart(lines[lineIndex + 1].Text)
                || CommentDefinitions.NDescStart(lines[lineIndex + 1].Text);
        }

        public override List<FortranObject> ReturnObject(IEnumerable<FortranObject> subObjects, List<FileLine> lines)
        {
            return new List<FortranObject> { new Description(lines) };
        }
    }

    public class DescriptionPostAction : PostAction
    {
        public DescriptionPostAction() : base(typeof(Description)) { }

        public override void PostObject(ref FortranObject obj)
        {
            // If this is a description directly below the definition statement then dont move it. This is really
            // just for function where the result name is the same as the function name.
            if (obj.parent.Identifier == obj.Identifier 
                && obj.parent.lines.Count > 1 && obj.parent.lines[1].Number == obj.lines[0].Number) return;

            string ident = obj.Identifier;
            var descriptions = (from sObjs in obj.parent.SubObjects
                        where sObjs.Identifier == ident
                        where sObjs.GetType() != typeof(Description)
                        select sObjs);

            var fortranObjects = descriptions as IList<FortranObject> ?? descriptions.ToList();

            if (fortranObjects.Count() > 1)
            {
                var curObj = obj;
                var file = obj.GoUpTillType<File>();
                Report.Warning(pub =>
                {
                    pub.AddWarningDescription("Description meta-data was ignored");
                    pub.AddReason("Description specified multiple times. Using first occurence");
                    pub.AddLocation(curObj.lines.First().Number == curObj.lines.Last().Number
                        ? $"At line {curObj.lines.First().Number} of '{file.Name}{file.Info.Extension}'."
                        : $"Within lines {curObj.lines.First().Number} to {curObj.lines.Last().Number} of '{file.Name}{file.Info.Extension}'.");
                });
            }

            FortranObject parSubObj = fortranObjects.FirstOrDefault();

            if (parSubObj != null)
            {
                obj.parent.SubObjects.Remove(obj);
                parSubObj.AddSubObject(obj);
            }
        }
    }

    public class DescriptionGroup : ObjectGroup
    {
        public DescriptionGroup()
            : base(typeof(Description)) { }

        public override XElement XEle(IEnumerable<XElement> content)
        {
            return content.SingleOrDefault();
        }
    }

    public class Description : XFortranObject
    {
        public string Basic { get; private set; }
        public string Detailed { get; private set; }

        private readonly Markdown _markdown = new Markdown();
        private string _identifier;

        public Description() { }

        public Description(string basicText)
            : base("Description", new List<FileLine> { new FileLine(-1, basicText) })
        {
            this.Basic = basicText;
        }

        public Description(string identifier, string basicText)
            : base("Description", new List<FileLine> { new FileLine(-1, basicText) })
        {
            this._identifier = identifier;
            this.Basic = basicText;
        }

        public Description(string identifier, string basicText, string detailedText, List<FileLine> lines)
            : base("Description", lines)
        {
            this._identifier = identifier;
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
            if (this._identifier == null) return this.parent.Identifier;
            else return this._identifier;
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
            var xele = new XElement(this.XElement_Name);

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