//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

using Doctran.Fbase.Common;

using Doctran.BaseClasses;

namespace Doctran.Fbase.Comments
{
    public class InformationBlock : FortranBlock
    {
        public InformationBlock()
            : base("Information", true, false, 1) { }

        public override bool BlockStart(String parent_block_name, List<FileLine> lines, int lineIndex)
        {
            return
                CommentDefinitions.InfoStart(lines[lineIndex].Text)
                && !CommentDefinitions.NDescStart(lines[lineIndex].Text)
                && !CommentDefinitions.DetailLine(lines[lineIndex].Text);
        }

        public override bool BlockEnd(String parent_block_name, List<FileLine> lines, int lineIndex)
        {
            if (lineIndex + 1 >= lines.Count) return true;
            return
                CommentDefinitions.InfoEnd(lines[lineIndex + 1].Text)
                || CommentDefinitions.InfoStart(lines[lineIndex + 1].Text)
                || CommentDefinitions.NDescStart(lines[lineIndex + 1].Text);
        }

        public override List<FortranObject> ReturnObject(IEnumerable<FortranObject> sub_objects, List<FileLine> lines)
        {
            Match aMatch = Regex.Match(lines[0].Text.Trim(), @"!>\s*(\w+)\s*:");
            String typeName = aMatch.Groups[1].Value.Trim();
            return new List<FortranObject>() { new Information(typeName, sub_objects, lines) };
        }
    }

    public class InformationGroup : ObjectGroup
    {
        public InformationGroup()
            : base(typeof(Information)) { }

        public override XElement XEle(IEnumerable<XElement> Content)
        {
            return new XElement("Information", Content);
        }
    }

    public class Information : XFortranObject
    {
        private String _value;

        public Information(String typeName, IEnumerable<FortranObject> sub_objects, List<FileLine> lines)
            : base(typeName, sub_objects, lines) 
        { 
            this.GetValue();
        }

        public Information(String typeName, String value)
            :base(typeName, new List<FileLine>())
        {
            this._value = value;
        }

        public Information(String typeName, String value, List<SubInformation> subInformation)
            : base(typeName, new List<FileLine>())
        {
            this._value = value;
            this.AddSubObjects(subInformation);
        }

        public String Type
        {
            get
            {
                return this.XElement_Name;
            }
        }

        public String Value
        {
            get
            {
                return _value;
            }
        }

        private void GetValue()
        {
            Match aMatch = Regex.Match(this.lines[0].Text.Trim(), @".*:(.*)");
            String value = aMatch.Groups[1].Value.Trim();
            if (value != "") this._value = value;
        }

        protected override String GetIdentifier()
        {
            return "FortranObject(Information)";
        }

        public override XElement XEle()
        {
            XElement xEle = new XElement(this.XElement_Name);
            if (this.Value != null) xEle.Value = this.Value;
            xEle.Add(
                from attr in this.SubObjectsOfType<SubInformation>()
                select attr.XEle()
                );
            return xEle;
        }
    }

    public class SubInformationBlock : FortranBlock
    {
        private InformationBlock infoBlock = new InformationBlock();

        public SubInformationBlock()
            : base("Sub-Information", false, false, 2) { }

        public override bool BlockStart(String parent_block_name, List<FileLine> lines, int lineIndex)
        {
            return Regex.IsMatch(lines[lineIndex].Text.Trim(), @"^!>>\s*\w+\s*:");
        }

        public override bool BlockEnd(String parent_block_name, List<FileLine> lines, int lineIndex)
        {
            if (lineIndex + 1 >= lines.Count) return true;
            return !Regex.IsMatch(lines[lineIndex + 1].Text.Trim(), @"^!>>")
                 | this.BlockStart(parent_block_name, lines, lineIndex + 1)
                 | infoBlock.BlockStart(parent_block_name, lines, lineIndex + 1);
        }

        public override List<FortranObject> ReturnObject(IEnumerable<FortranObject> sub_objects, List<FileLine> lines)
        {
            Match aMatch = Regex.Match(lines[0].Text.Trim(), @"!>>(.*):");
            String TypeName = aMatch.Groups[1].Value.Trim();
            return new List<FortranObject>() { new SubInformation(TypeName, lines) };
        }
    }

    public class SubInformationGroup : ObjectGroup
    {
        public SubInformationGroup()
            : base(typeof(SubInformation)) { }

        public override XElement XEle(IEnumerable<XElement> Content)
        {
            return Content.Single();
        }
    }

    public class SubInformation : XFortranObject
    {
        private String _value;

        public SubInformation(String typeName, List<FileLine> lines)
            : base(typeName, lines) 
        {
            this.GetValue();
        }

        public SubInformation(String typeName, String value)
            : base(typeName, new List<FileLine>()) {
                this._value = value;
        }

        public String Type
        {
            get
            {
                return this.XElement_Name;
            }
        }

        private void GetValue()
        {
            Match aMatch = Regex.Match(lines[0].Text.Trim(), @"^!>>\s*\w+\s*:(.*)");
            String value = aMatch.Groups[1].Value.Trim();
            var temp = (from line in this.lines.Skip(1)
                        select line.Text.Substring(3).Trim()).ToArray();
            value = value + String.Join(" ", temp);

            if (value != "") this._value = value;
        }

        public String Value
        {
            get
            {
                return _value;
            }
        }

        protected override String GetIdentifier()
        {
            return "FortranObject(SubInformation)";
        }

        public override XElement XEle()
        {
            XElement xEle = new XElement(this.XElement_Name);
            if (this.Value != null) xEle.Value = this.Value;
            return XElement.Parse("<" + this.Type + ">" + this.Value + "</" + this.Type + ">");
        }
    }

}