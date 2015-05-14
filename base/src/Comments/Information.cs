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
        {
            this.CheckInternal = false;
            this.Weight = 1;
        }

        public override bool BlockStart(Type parentType, List<FileLine> lines, int lineIndex)
        {
            return
                CommentDefinitions.InfoStart(lines[lineIndex].Text)
                && !CommentDefinitions.NDescStart(lines[lineIndex].Text)
                && !CommentDefinitions.DetailLine(lines[lineIndex].Text);
        }

        public override bool BlockEnd(Type parentType, List<FileLine> lines, int lineIndex)
        {
            if (lineIndex + 1 >= lines.Count) return true;

            return
                CommentDefinitions.InfoEnd(lines[lineIndex + 1].Text)
                || CommentDefinitions.NDescStart(lines[lineIndex + 1].Text);
        }

        public override List<FortranObject> ReturnObject(FortranObject parent, List<FileLine> lines)
        {
            Match aMatch = Regex.Match(lines[0].Text.Trim(), @"!>(.*):");
            String typeName = aMatch.Groups[1].Value.Trim();
            return new List<FortranObject>() { new Information(parent, typeName, lines) };
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

        public Information(FortranObject parent, String typeName,List<FileLine> lines)
            : base(parent, typeName, lines, true) 
        { 
            this.GetValue();
        }

        public Information(FortranObject parent, String typeName, String value)
            :base(parent, typeName, new List<FileLine>(), true)
        {
            this._value = value;
        }

        public Information(FortranObject parent, String typeName, String value, List<SubInformation> subInformation)
            : base(parent, typeName, new List<FileLine>(), true)
        {
            this._value = value;
            this.SubObjects.AddRange(subInformation);
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
        {
            this.CheckInternal = false;
            this.Weight = 2;
        }

        public override bool BlockStart(Type parentType, List<FileLine> lines, int lineIndex)
        {
            return Regex.IsMatch(lines[lineIndex].Text.Trim(), @"^!>>\s*\w+\s*:");
        }

        public override bool BlockEnd(Type parentType, List<FileLine> lines, int lineIndex)
        {
            if (lineIndex + 1 >= lines.Count) return true;
            return !Regex.IsMatch(lines[lineIndex + 1].Text.Trim(), @"^!>>")
                 | this.BlockStart(parentType, lines, lineIndex + 1)
                 | infoBlock.BlockStart(parentType, lines, lineIndex + 1);
        }

        public override List<FortranObject> ReturnObject(FortranObject parent, List<FileLine> lines)
        {
            Match aMatch = Regex.Match(lines[0].Text.Trim(), @"!>>(.*):");
            String TypeName = aMatch.Groups[1].Value.Trim();
            return new List<FortranObject>() { new SubInformation(parent, TypeName, lines) };
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

        public SubInformation(FortranObject parent, String typeName, List<FileLine> lines)
            : base(parent, typeName, lines, false) 
        {
            this.GetValue();
        }

        public SubInformation(FortranObject parent, String typeName, String value)
            : base(parent, typeName, new List<FileLine>(), false) {
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