namespace Doctran.Test.TestingUtilities
{
    using System.Collections.Generic;
    using System.Linq;
    using Doctran.Helper;

    public class StringUtils
    {
        public static List<FileLine> ConvertToFileLineList(string linesString)
        {
            var lines = new List<FileLine>();
            lines.AddRange(
                linesString
                    .Split('\n')
                    .Select((l, i) => new FileLine(i, l))
                );
            return lines;
        }
    }
}