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
    public class NamedDescriptionBlock : FortranBlock
    {
        private InformationBlock infoBlock = new InformationBlock();

        public NamedDescriptionBlock() 
        {
            this.CheckInternal = false;
            this.Weight = 0;
        }

        public override bool BlockStart(Type parentType, List<FileLine> lines, int lineIndex)
        {
            return 
                    Regex.IsMatch(lines[lineIndex].Text.Trim(), @"!>\s*\w.*\s*-.*")
                    && !infoBlock.BlockStart(parentType, lines, lineIndex);
        }

        public override bool BlockEnd(Type parentType, List<FileLine> lines, int lineIndex)
        {
            if(lineIndex + 1 >= lines.Count) return true;
            return 
                   !lines[lineIndex + 1].Text.Trim().StartsWith("!>")
                | this.BlockStart(parentType, lines, lineIndex + 1)
                | infoBlock.BlockStart(parentType, lines, lineIndex + 1);
        }

        public override List<FortranObject> ReturnObject(FortranObject parent, List<FileLine> lines)
        {
            return new NamedDescription(parent, lines);
        }
    }

	public class NamedDescriptionGroup : ObjectGroup
    {
        public NamedDescriptionGroup()
            : base(typeof(NamedDescription)) { }

        public override XElement XEle(IEnumerable<XElement> Content)
        {
            return Content.Single();
        }
    }

    public class NamedDescription : List<FortranObject>
    {
        public NamedDescription(FortranObject parent, List<FileLine> lines)
        {
            Match aMatch = Regex.Match(lines[0].Text, @"!>\s*(\w.*)\s*-(.*)");
            String name = aMatch.Groups[1].Value.Trim();
            StringBuilder basic = new StringBuilder(aMatch.Groups[2].Value.Trim()).Append(
                String.Concat(
                from line in lines.Skip(1)
                where Regex.IsMatch(line.Text, @"^\s*!>")
                where !Regex.IsMatch(line.Text, @"^\s*!>>")
                select " " + line.Text.Trim().Substring(3)));

            String detailed = String.Concat(
                from line in lines
                where Regex.IsMatch(line.Text, @"^\s*!>>")
                select " " + line.Text.Trim().Substring(4));

            this.Add(new Description(parent, name, basic.ToString(), detailed, lines));
            
        }
    }

}