﻿//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
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
    public class DescriptionException : PostAction
    {
        public DescriptionException()
            : base(typeof(Description))
        { }

        public override void PostObject(ref FortranObject obj)
        {
            this.correctName(ref obj);
            this.checkUniqueness(ref obj);
        }

        public void correctName(ref FortranObject obj)
        {
            if (obj.parent.Identifier != obj.Identifier)
            {
                var file = obj.GoUpTillType<File>();
                UserInformer.GiveWarning(file.Name + file.Info.Extension, obj.lines.First().Number, obj.lines.Last().Number
                                        , "Description identifier does not match parent identifier and has been ignored.");
                obj.parent.SubObjects.Remove(obj);
            }
        }

        public void checkUniqueness(ref FortranObject obj)
        {
            if (obj.parent.SubObjectsOfType<Description>().Count > 1)
            {
                if (obj.parent is Project)
                {
                    UserInformer.GiveWarning("project file", obj.lines.First().Number, obj.lines.Last().Number
                                                                            , "Description is not unique and has been ignored.");
                }
                else
                {
                    var file = obj.GoUpTillType<File>();
                    UserInformer.GiveWarning(file.Name + file.Info.Extension, obj.lines.First().Number, obj.lines.Last().Number
                                                                            , "Description is not unique and has been ignored.");
                }
                obj.parent.SubObjects.Remove(obj);
            }
        }
    }
}

namespace Doctran.Fbase.Comments
{
    public class DescriptionBlock : FortranBlock
    {
        public DescriptionBlock()
            : base("Description", false, false, 3) { }

        public static String BlockName
        {
            get
            {
                return "Description";
            }
        }

        public override bool BlockStart(String parent_block_name, List<FileLine> lines, int lineIndex)
        {
            if (parent_block_name == DescriptionBlock.BlockName) return false;
            return
                CommentDefinitions.DescStart(lines[lineIndex].Text)
                && !CommentDefinitions.DetailLine(lines[lineIndex].Text)
                && !CommentDefinitions.NDescStart(lines[lineIndex].Text)
                && !CommentDefinitions.InfoStart(lines[lineIndex].Text);
        }

        public override bool BlockEnd(String parent_block_name, List<FileLine> lines, int lineIndex)
        {
            if (lines.Count == lineIndex + 1) return true;
            return
                CommentDefinitions.DescEnd(lines[lineIndex + 1].Text)
                || CommentDefinitions.InfoStart(lines[lineIndex + 1].Text)
                || CommentDefinitions.NDescStart(lines[lineIndex + 1].Text);
        }

        public override List<FortranObject> ReturnObject(IEnumerable<FortranObject> sub_objects, List<FileLine> lines)
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

            String ident = obj.Identifier;
            var descriptions = (from sObjs in obj.parent.SubObjects
                        where sObjs.Identifier == ident
                        where sObjs.GetType() != typeof(Description)
                        select sObjs);

            FortranObject parSubObj = descriptions.FirstOrDefault();
            if (descriptions.Count() > 1) UserInformer.GiveWarning(parSubObj.Name, "Description specified multiple times. Using first occurence");

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

        public override XElement XEle(IEnumerable<XElement> Content)
        {
            return Content.SingleOrDefault();
        }
    }

    public class Description : XFortranObject
    {
        public String Basic { get; private set; }
        public String Detailed { get; private set; }

        private readonly Markdown _markdown = new Markdown();
        private String identifier;

        public Description() { }

        public Description(String basicText)
            : base("Description", new List<FileLine> { new FileLine(-1, basicText) })
        {
            this.Basic = basicText;
        }

        public Description(String identifier, String basicText)
            : base("Description", new List<FileLine> { new FileLine(-1, basicText) })
        {
            this.identifier = identifier;
            this.Basic = basicText;
        }

        public Description(String identifier, String basicText, String detailedText, List<FileLine> lines)
            : base("Description", lines)
        {
            this.identifier = identifier;
            this.Basic = basicText;
            this.Detailed = detailedText;
        }

        public Description(List<FileLine> lines)
            : base("Description", lines)
        {
            this.Basic = String.Concat(
                from line in lines
                where Regex.IsMatch(line.Text, @"^\s*!>") && !Regex.IsMatch(line.Text, @"^\s*!>>")
                select (Regex.Match(line.Text, @"!>(.*)").Groups[1].Value.Trim()) + " ").TrimEnd();
            this.Detailed = MergeLines(lines);
        }

        public static String MergeLines(List<FileLine> lines)
        {
            return String.Concat(
                from line in lines
                where Regex.IsMatch(line.Text, @"^\s*!>>(.*)")
                select (Regex.Match(line.Text, @"!>>(.*)").Groups[1].Value) + "\n");
        }

        protected override String GetIdentifier()
        {
            if (this.identifier == null) return this.parent.Identifier;
            else return this.identifier;
        }

        protected XElement parse(String name, String text)
        {
            try
            {
                return XElement.Parse("<" + name + ">" + text + "</" + name + ">", LoadOptions.PreserveWhitespace);
            }
            catch
            {
                var file = this.GoUpTillType<Files.File>();

                UserInformer.GiveWarning(file.Name + file.Info.Extension, this.lines.First().Number, this.lines.Last().Number, name + " description could not be parsed and was ignored.");
                return new XElement(name);
            }
        }

        public override XElement XEle()
        {
            XElement xele = new XElement(this.XElement_Name);

            xele.Add(this.parse("Basic", WebUtility.HtmlEncode(this.Basic)));

            if (this.Detailed != null && this.Detailed != "")
            {
                var project = this.GoUpTillType<Projects.Project>();
                XElement detailed;
                if (project.UseText) detailed = new XElement("Detailed", this.parse("Text", "<![CDATA[" + this.Detailed + "]]>"));
                else if (project.UseHtml) detailed = this.parse("Detailed", "<Html>" + this.Detailed + @"</Html>");
                else detailed = this.parse("Detailed", "<Html>" + _markdown.Transform(this.Detailed) + @"</Html>");
                xele.Add(detailed);
            }
            return xele;
        }
    }

}