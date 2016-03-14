// <copyright file="NamedDescriptionBlock.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.ParsingElements.FortranBlocks
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml;
    using System.Xml.Linq;
    using FortranObjects;
    using Helper;
    using Parsing;
    using Utilitys;

    public class NamedDescriptionBlock : IFortranBlock
    {
        public bool CheckInternal => false;

        public bool ExplicitEnd => false;

        public string Name => "Named Description";

        public bool BlockEnd(IEnumerable<IFortranBlock> ancestors, List<FileLine> lines, int lineIndex)
        {
            if (lineIndex + 1 >= lines.Count)
            {
                return true;
            }

            var parentAncestors = ancestors.Skip(1);
            return
                CommentUtils.NDescEnd(lines[lineIndex + 1].Text)
                || this.BlockStart(parentAncestors, lines, lineIndex + 1)
                || CommentUtils.InfoStart(lines[lineIndex + 1].Text);
        }

        public bool BlockStart(IEnumerable<IFortranBlock> ancestors, List<FileLine> lines, int lineIndex)
        {
            var parentName = ancestors.FirstOrDefault()?.Name;
            return
                CommentUtils.NDescStart(lines[lineIndex].Text)
                && !(parentName ?? string.Empty).StartsWith("Information_")
                && !CommentUtils.InfoStart(lines[lineIndex].Text);
        }

        public IEnumerable<IContained> ReturnObject(IEnumerable<IContained> subObjects, List<FileLine> lines)
        {
            var result = Regex.Match(lines[0].Text, @"(!>>?\s*)(\w.*)\s*-(.*)");
            var symbol = result.Groups[1].Value;
            var name = result.Groups[2].Value.Trim();
            var value = result.Groups[3].Value;

            var newLines = new List<FileLine>()
            {
                new FileLine(lines.First().Number, symbol + value)
            };
            newLines.AddRange(lines.Skip(1));

            var basic = XmlUtils.WrapAsCData("Basic", DescriptionBlock.GetBasicText(newLines));
            var detailText = DescriptionBlock.GetDetailText(newLines);

            try
            {
                var detailed = XmlUtils.WrapAndParse("Detailed", detailText);
                return new NamedDescription(name.ToLower(), basic, detailed, lines).Singlet();
            }
            catch (XmlException e)
            {
                throw new BlockParserException($"{e.Message.TrimEnd('.')} in the following generated markdown string: '{detailText.TrimEnd('\n')}'.");
            }
        }
    }
}