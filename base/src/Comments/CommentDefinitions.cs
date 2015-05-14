using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Doctran.Fbase.Comments
{
    internal static class CommentDefinitions
    {
        public static bool DescStart(String text) { return Regex.IsMatch(text, @"^\s*!>"); }

        public static bool DescEnd(String text) { return !Regex.IsMatch(text, @"^\s*!>"); }

        public static bool NDescStart(String text) { return Regex.IsMatch(text.Trim(), @"!>\s*\w.*\s*-.*"); }

        public static bool NDescEnd(String text) { return !Regex.IsMatch(text, @"^\s*!>"); }

        public static bool InfoStart(String text) { return Regex.IsMatch(text.Trim(), @"^!>\s*\w+\s*:"); }

        public static bool InfoEnd(String text) { return !Regex.IsMatch(text, @"^\s*!>>"); }

        public static bool DetailLine(String text) { return Regex.IsMatch(text, @"^\s*!>>"); }
    }
}