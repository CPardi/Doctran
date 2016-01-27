// <copyright file="DescriptionBlock.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.ParsingElements.FortranBlocks
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Xml;
    using FortranObjects;
    using Helper;
    using MarkdownSharp;
    using Parsing;
    using Utilitys;

    public class DescriptionBlock : IFortranBlock
    {
        public bool CheckInternal => false;

        public bool ExplicitEnd => false;

        public string Name => "Description";

        public static string GetBasicText(IEnumerable<FileLine> lines)
        {
            return
                string.Concat(
                    from line in lines
                    where Regex.IsMatch(line.Text, @"^\s*!>") && !Regex.IsMatch(line.Text, @"^\s*!>>")
                    select Regex.Match(line.Text, @"!>(.*)").Groups[1].Value.TrimStart()) + " "
                        .TrimEnd();
        }

        public static string GetDetailText(List<FileLine> lines)
        {
            var detailLines = lines
                .Where(line => Regex.IsMatch(line.Text, @"^\s*!>>(.*)"))
                .ToList();

            return new Markdown().Transform(
                string.Concat(
                    detailLines
                        .Select(line => Regex.Match(line.Text, @"!>>(.*)").Groups[1].Value)
                        .Select((line, i) => i == detailLines.Count - 1 ? line : $"{line}\n")));
        }

        public bool BlockEnd(IEnumerable<IFortranBlock> ancestors, List<FileLine> lines, int lineIndex)
        {
            if (lines.Count == lineIndex + 1)
            {
                return true;
            }

            return
                CommentUtils.DescEnd(lines[lineIndex + 1].Text)
                || CommentUtils.InfoStart(lines[lineIndex + 1].Text)
                || CommentUtils.NDescStart(lines[lineIndex + 1].Text);
        }

        public bool BlockStart(IEnumerable<IFortranBlock> ancestors, List<FileLine> lines, int lineIndex)
        {
            var parentName = ancestors.FirstOrDefault()?.Name;

            if (parentName == this.Name)
            {
                return false;
            }

            return
                CommentUtils.DescStart(lines[lineIndex].Text)
                && !(parentName ?? string.Empty).StartsWith("Information_")
                && !CommentUtils.NDescStart(lines[lineIndex].Text)
                && !CommentUtils.InfoStart(lines[lineIndex].Text);
        }

        public IEnumerable<IContained> ReturnObject(IEnumerable<IContained> subObjects, List<FileLine> lines)
        {
            var basic = XmlUtils.WrapAndParse("Basic", GetBasicText(lines));
            var detailText = GetDetailText(lines);

            try
            {
                var detailed = XmlUtils.WrapAndParse("Detailed", detailText);
                return CollectionUtils.Singlet(new Description(basic, detailed, lines));
            }
            catch (XmlException e)
            {
                throw new BlockParserException($"{e.Message.TrimEnd('.')} in the following generated markdown string: '{detailText.TrimEnd('\n')}'.");
            }
        }
    }
}