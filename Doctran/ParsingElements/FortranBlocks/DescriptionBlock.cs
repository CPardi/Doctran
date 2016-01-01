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
    using FortranObjects;
    using Helper;
    using MarkdownSharp;
    using Parsing;
    using Utilitys;

    public class DescriptionBlock : IFortranBlock
    {
        private static readonly Markdown Markdown = new Markdown();

        public bool CheckInternal => false;

        public bool ExplicitEnd => false;

        public string Name => "Description";

        public static string GetBasicText(IEnumerable<FileLine> lines)
        {
            return WebUtility.HtmlEncode(
                string.Concat(
                    from line in lines
                    where Regex.IsMatch(line.Text, @"^\s*!>") && !Regex.IsMatch(line.Text, @"^\s*!>>")
                    select Regex.Match(line.Text, @"!>(.*)").Groups[1].Value.Trim()) + " "
                        .TrimEnd());
        }

        public static string GetDetailText(List<FileLine> lines)
        {
            return Markdown.Transform(
                string.Concat(
                    from line in lines
                    where Regex.IsMatch(line.Text, @"^\s*!>>(.*)")
                    select Regex.Match(line.Text, @"!>>(.*)").Groups[1].Value + "\n"));
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
                && !CommentUtils.DetailLine(lines[lineIndex].Text)
                && !CommentUtils.NDescStart(lines[lineIndex].Text)
                && !CommentUtils.InfoStart(lines[lineIndex].Text);
        }

        public IEnumerable<FortranObject> ReturnObject(IEnumerable<IContained> subObjects, List<FileLine> lines)
        {
            var basic = XmlUtils.WrapAndParse("Basic", GetBasicText(lines));
            var detailed = XmlUtils.WrapAndParse("Detailed", GetDetailText(lines));

            yield return new Description(basic, detailed, lines);
        }
    }
}