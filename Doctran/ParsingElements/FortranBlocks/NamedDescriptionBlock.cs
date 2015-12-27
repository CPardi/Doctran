// <copyright file="NamedDescriptionBlock.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing.BuiltIn.FortranBlocks
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;
    using FortranObjects;
    using Helper;
    using Utilitys;

    public class NamedDescriptionBlock : FortranBlock
    {
        public bool CheckInternal => false;

        public bool ExplicitEnd => false;

        public string Name => "Named Description";

        public  bool BlockEnd(string parentBlockName, List<FileLine> lines, int lineIndex)
        {
            if (lineIndex + 1 >= lines.Count)
            {
                return true;
            }
            return
                CommentUtils.NDescEnd(lines[lineIndex + 1].Text)
                || this.BlockStart(parentBlockName, lines, lineIndex + 1)
                || CommentUtils.InfoStart(lines[lineIndex + 1].Text);
        }

        public  bool BlockStart(string parentBlockName, List<FileLine> lines, int lineIndex)
        {
            return
                CommentUtils.NDescStart(lines[lineIndex].Text)
                && !parentBlockName.StartsWith("Information_")
                && !CommentUtils.DetailLine(lines[lineIndex].Text)
                && !CommentUtils.InfoStart(lines[lineIndex].Text);
        }

        public  IEnumerable<FortranObject> ReturnObject(IEnumerable<IFortranObject> subObjects, List<FileLine> lines)
        {
            var name = Regex.Match(lines[0].Text, @"!>\s*(\w.*)\s*-").Groups[1].Value.Trim();
            var basic = XmlUtils.WrapAndParse("Basic", DescriptionBlock.GetBasicText(lines).Substring(name.Length + 1).TrimStart(' ', '-'));
            var detailed = XmlUtils.WrapAndParse("Detailed", DescriptionBlock.GetDetailText(lines));
            yield return new NamedDescription(name.ToLower(), basic, detailed, lines); 
        }
    }
}