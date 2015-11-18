namespace Doctran.Parsing.BuiltIn.FortranBlocks
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;
    using FortranObjects;
    using Helper;
    using MarkdownSharp;
    using Parsing;
    using Utilitys;

    public class DescriptionBlock : FortranBlock
    {
        public DescriptionBlock()
            : base("Description", false, false, 3) { }

        public static string BlockName => "Description";

        public override bool BlockStart(string parentBlockName, List<FileLine> lines, int lineIndex)
        {
            if (parentBlockName == DescriptionBlock.BlockName) return false;
            return
                CommentUtils.DescStart(lines[lineIndex].Text)
                && !parentBlockName.StartsWith("Information_")
                && !CommentUtils.DetailLine(lines[lineIndex].Text)
                && !CommentUtils.NDescStart(lines[lineIndex].Text)
                && !CommentUtils.InfoStart(lines[lineIndex].Text);
        }

        public override bool BlockEnd(string parentBlockName, List<FileLine> lines, int lineIndex)
        {
            if (lines.Count == lineIndex + 1) return true;
            return
                CommentUtils.DescEnd(lines[lineIndex + 1].Text)
                || CommentUtils.InfoStart(lines[lineIndex + 1].Text)
                || CommentUtils.NDescStart(lines[lineIndex + 1].Text);
        }

        public override IEnumerable<FortranObject> ReturnObject(IEnumerable<FortranObject> subObjects, List<FileLine> lines)
        {
            var basic = DescriptionBlock.ParseXmlContent("Basic", DescriptionBlock.GetBasicText(lines));
            var detailed = DescriptionBlock.ParseXmlContent("Detailed", DescriptionBlock.GetDetailText(lines));

            yield return new Description2(basic, detailed, lines);
        }

        private static readonly Markdown Markdown = new Markdown();

        public static string GetBasicText(IEnumerable<FileLine> lines)
        {
            return WebUtility.HtmlEncode(
                string.Concat(
                    from line in lines
                    where Regex.IsMatch(line.Text, @"^\s*!>") && !Regex.IsMatch(line.Text, @"^\s*!>>")
                    select Regex.Match(line.Text, @"!>(.*)").Groups[1].Value.Trim()) + " "
                    .TrimEnd()
                    );
        }

        public static string GetDetailText(List<FileLine> lines)
        {
            return Markdown.Transform(
                string.Concat(
                    from line in lines
                    where Regex.IsMatch(line.Text, @"^\s*!>>(.*)")
                    select (Regex.Match(line.Text, @"!>>(.*)").Groups[1].Value) + "\n")
                );
        }

        private static XElement ParseXmlContent(string name, string text)
        {
            return XElement.Parse("<" + name + ">" + text + "</" + name + ">", LoadOptions.PreserveWhitespace);            
        }
    }
}