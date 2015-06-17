//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.IO;
using MarkdownSharp;

namespace Doctran.Fbase.Common
{
    public static class Helper
    {
        public static readonly Markdown markdown = new Markdown();

        public static void Stop()
        {
            Environment.Exit(1);
        }

        public static string RemoveInlineComment(String s)
        {
            return s.Split('!')[0].Trim();
        }

        public static string ToUpperFirstLowerRest(this String s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static void CreateDirectory(String relativePath)
        {
            if (!Directory.Exists(Path.GetFullPath(relativePath)))
            { Directory.CreateDirectory(Path.GetFullPath(relativePath)); }
        }

        /// <summary>
        /// Moves to the next code line. This means comments and doctran comments are skipped.
        /// </summary>
        public static void SkipComment(List<FileLine> lines, ref int lineNumber)
        {
            while (lines[lineNumber].Text.Trim().StartsWith("!")) { lineNumber++; }
        }

        /// <summary>
        /// Moves to the next line in the file which is not white space nor a fortran Description.
        /// </summary>
        public static bool GotoNextUsefulLine(List<FileLine> lines, ref int lineIndex)
        {
            if (lineIndex >= lines.Count) return true;
            String text_nowhitespace;
            while (
                (text_nowhitespace = lines[lineIndex].Text.Trim()) == ""
                | (text_nowhitespace.StartsWith("!") & !text_nowhitespace.StartsWith("!>"))
                    ) { lineIndex++; if (lineIndex >= lines.Count) break; }
            return lineIndex == lines.Count;
        }

        public static String NoWhitespace(String Text)
        {
            return Regex.Replace(Text, @"\s+", "");
        }

        public static List<String> DelimiterExceptBrackets(String text, char delimiter)
        {
            List<String> DelimiteredText = new List<string>();
            int WithinBracketsDepth = 0;
            int PrevIndex = 0, CurrentIndex = 0;
            foreach (char aChar in text)
            {
                if (aChar == '(' | aChar == '[') { WithinBracketsDepth++; }
                if (aChar == ')' | aChar == ']') WithinBracketsDepth--;
                if (WithinBracketsDepth == 0 & aChar == delimiter)
                {
                    DelimiteredText.Add(text.Substring(PrevIndex, CurrentIndex - PrevIndex).Trim());
                    PrevIndex = CurrentIndex + 1;
                }
                if (CurrentIndex == text.Count() - 1)
                {
                    DelimiteredText.Add(text.Substring(PrevIndex, CurrentIndex - PrevIndex + 1).Trim());
                }
                CurrentIndex++;
            }
            return DelimiteredText;
        }

        public static List<String> DelimiterExceptQuotes(String text, char delimiter)
        {
            List<String> DelimiteredText = new List<string>();
            bool s_quotes = false;
            bool d_quotes = false;
            int PrevIndex = 0, CurrentIndex = 0;
            foreach (char aChar in text)
            {
                if (!(s_quotes | d_quotes))
                {
                    s_quotes = aChar == '\'';
                    d_quotes = aChar == '"';
                }
                else
                {
                    if (s_quotes) s_quotes = !(aChar == '\'');
                    if (d_quotes) d_quotes = !(aChar == '"');
                }

                if (!(s_quotes | d_quotes) && aChar == delimiter)
                {
                    DelimiteredText.Add(text.Substring(PrevIndex, CurrentIndex - PrevIndex).Trim());
                    PrevIndex = CurrentIndex + 1;
                }
                if (CurrentIndex == text.Length - 1)
                {
                    DelimiteredText.Add(text.Substring(PrevIndex, CurrentIndex - PrevIndex + 1).Trim());
                }
                CurrentIndex++;
            }
            return DelimiteredText;
        }

        public static String ValidName(String name)
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

        public static XElement XEle(this DateTime value)
        {
            return new XElement("DateTime", value.ToString("o"));
        }
    }
}
