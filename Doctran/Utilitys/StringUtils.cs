namespace Doctran.Utilitys
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class StringUtils
    {
        public static List<string> DelimiterExceptBrackets(string text, char delimiter)
        {
            var delimiteredText = new List<string>();
            var withinBracketsDepth = 0;
            int prevIndex = 0, currentIndex = 0;
            foreach (var aChar in text)
            {
                if (aChar == '(' | aChar == '[')
                {
                    withinBracketsDepth++;
                }
                if (aChar == ')' | aChar == ']') withinBracketsDepth--;
                if (withinBracketsDepth == 0 & aChar == delimiter)
                {
                    delimiteredText.Add(text.Substring(prevIndex, currentIndex - prevIndex).Trim());
                    prevIndex = currentIndex + 1;
                }
                if (currentIndex == text.Count() - 1)
                {
                    delimiteredText.Add(text.Substring(prevIndex, currentIndex - prevIndex + 1).Trim());
                }
                currentIndex++;
            }
            return delimiteredText;
        }

        public static List<string> DelimiterExceptQuotes(string text, char delimiter)
        {
            var delimiteredText = new List<string>();
            var sQuotes = false;
            var dQuotes = false;
            int prevIndex = 0, currentIndex = 0;
            foreach (var aChar in text)
            {
                if (!(sQuotes | dQuotes))
                {
                    sQuotes = aChar == '\'';
                    dQuotes = aChar == '"';
                }
                else
                {
                    if (sQuotes) sQuotes = aChar != '\'';
                    if (dQuotes) dQuotes = aChar != '"';
                }

                if (!(sQuotes | dQuotes) && aChar == delimiter)
                {
                    delimiteredText.Add(text.Substring(prevIndex, currentIndex - prevIndex).Trim());
                    prevIndex = currentIndex + 1;
                }

                if (currentIndex == text.Length - 1)
                {
                    delimiteredText.Add(text.Substring(prevIndex, currentIndex - prevIndex + 1).Trim());
                }

                currentIndex++;
            }

            return delimiteredText;
        }

        public static string NoWhitespace(string text)
        {
            return Regex.Replace(text, @"\s+", "");
        }

        public static string RemoveInlineComment(string s)
        {
            return s.Split('!')[0].Trim();
        }

        public static string ToUpperFirstLowerRest(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static string ValidName(string name)
        {
            return name.Replace('+', 'p')
                .Replace('-', 'm')
                .Replace('/', 'd')
                .Replace('\\', 'V')
                .Replace('*', 't')
                .Replace('=', 'e')
                .Replace('.', 'o')
                .Replace('<', 'l')
                .Replace('>', 'g');
        }
    }
}