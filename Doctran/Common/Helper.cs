//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.IO;
using MarkdownSharp;

namespace Doctran.Fbase.Common
{
    public static class Helper
    {
        public static IEnumerable<T> Singlet<T>(T tInstance)
        {
            yield return tInstance;
        }

        public static T GetAssemblyAttribute<T>(this System.Reflection.Assembly ass) where T : Attribute
        {
            object[] attributes = ass.GetCustomAttributes(typeof(T), false);
            if (attributes == null || attributes.Length == 0)
                return null;
            return attributes.OfType<T>().Single();
        }

        /// <summary>
        /// Return a html file path and html content for a html or markdown file.
        /// </summary>
        /// <param name="file_path">The path to the html or markdown file.</param>
        /// <returns>In the first string is the file path for the html file. In the second string in the html content that the file should contain.</returns>
        public static Tuple<string, string> GetMarkUpFile(string relative_path, string file_path)
        {
            Markdown mdParser = new Markdown();

            // Store the file's path and contents in these variables initially, and if its a markdown file then reassign after.
            string html_path = file_path;
            var html_text = "";

            // Get the text from the file specified.
            try
            {
                html_text = string.Concat(from text in Files.File.ReadFile(relative_path + file_path)
                                          select text.Text + Environment.NewLine);
            }
            catch { UserInformer.GiveError("Project File", "File not found at \"" + file_path + "\""); }

            // Check if a Markdown is specified and if so parse it to get the html. The Path information should specified the 
            // path in the documentation to the file. So change its externsion to html.
            var ext = System.IO.Path.GetExtension(file_path);
            if (ext == ".md" || ext == ".markdown")
            {
                html_path = file_path.Remove(file_path.LastIndexOf('.')) + ".html";
                html_text = mdParser.Transform(html_text);
            }

            return new Tuple<string, string>(html_path, html_text);
        }

        public static string RegexMultipleOr(IEnumerable<string> regexes)
        {
            return regexes.First() + string.Concat(regexes.Skip(1).Select(d => "|" + d));
        }

        public static void Stop()
        {
            Environment.Exit(1);
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

        public static void CreateDirectory(string relativePath)
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
            string text_nowhitespace;
            while (
                (text_nowhitespace = lines[lineIndex].Text.Trim()) == ""
                | (text_nowhitespace.StartsWith("!") & !text_nowhitespace.StartsWith("!>"))
                    ) { lineIndex++; if (lineIndex >= lines.Count) break; }
            return lineIndex == lines.Count;
        }

        public static string NoWhitespace(string Text)
        {
            return Regex.Replace(Text, @"\s+", "");
        }

        public static List<string> DelimiterExceptBrackets(string text, char delimiter)
        {
            List<string> DelimiteredText = new List<string>();
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

        public static List<string> DelimiterExceptQuotes(string text, char delimiter)
        {
            List<string> DelimiteredText = new List<string>();
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

        public static XElement XEle(this DateTime value)
        {
            return new XElement("DateTime", value.ToString("o"));
        }
    }
}
