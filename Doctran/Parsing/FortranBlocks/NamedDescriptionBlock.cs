namespace Doctran.Parsing.FortranBlocks
{
    using System.Collections.Generic;
    using FortranObjects;
    using Helper;
    using Parsing;
    using Utilitys;

    public class NamedDescriptionBlock : FortranBlock
    {
        public NamedDescriptionBlock()
            : base("Named Description", false, false, 0) { }

        public override bool BlockStart(string parentBlockName, List<FileLine> lines, int lineIndex)
        {
            return
                CommentUtilitys.NDescStart(lines[lineIndex].Text)
                && !parentBlockName.StartsWith("Information_")
                && !CommentUtilitys.DetailLine(lines[lineIndex].Text)
                && !CommentUtilitys.InfoStart(lines[lineIndex].Text);
        }

        public override bool BlockEnd(string parentBlockName, List<FileLine> lines, int lineIndex)
        {
            if(lineIndex + 1 >= lines.Count) return true;
            return
                CommentUtilitys.NDescEnd(lines[lineIndex + 1].Text)
                || this.BlockStart(parentBlockName, lines, lineIndex + 1)
                || CommentUtilitys.InfoStart(lines[lineIndex + 1].Text);
        }

        public override List<FortranObject> ReturnObject(IEnumerable<FortranObject> subObjects, List<FileLine> lines)
        {
            return new NamedDescription(lines);
        }
    }
}