namespace Doctran.Utilitys
{
    using System.Collections.Generic;
    using System.Linq;
    using Helper;

    public static class PreprocessorUtils
    {
        public static List<FileLine> TrimLines(List<FileLine> lines, string currentDirectory)
        {
            return lines
                .Select(line => new FileLine(line.Number, line.Text.Trim()))
                .ToList();
        }
    }
}