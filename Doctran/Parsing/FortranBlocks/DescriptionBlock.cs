namespace Doctran.Parsing.FortranBlocks
{
    using System.Collections.Generic;
    using FortranObjects;
    using Helper;
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
                CommentUtilitys.DescStart(lines[lineIndex].Text)
                && !parentBlockName.StartsWith("Information_")
                && !CommentUtilitys.DetailLine(lines[lineIndex].Text)
                && !CommentUtilitys.NDescStart(lines[lineIndex].Text)
                && !CommentUtilitys.InfoStart(lines[lineIndex].Text);
        }

        public override bool BlockEnd(string parentBlockName, List<FileLine> lines, int lineIndex)
        {
            if (lines.Count == lineIndex + 1) return true;
            return
                CommentUtilitys.DescEnd(lines[lineIndex + 1].Text)
                || CommentUtilitys.InfoStart(lines[lineIndex + 1].Text)
                || CommentUtilitys.NDescStart(lines[lineIndex + 1].Text);
        }

        public override List<FortranObject> ReturnObject(IEnumerable<FortranObject> subObjects, List<FileLine> lines)
        {
            return new List<FortranObject> { new Description(lines) };
        }
    }
}