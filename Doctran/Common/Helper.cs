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
    using Reporting;

    public static class HelperUtils
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
        /// <param name="filePath">The path to the html or markdown file.</param>
        /// <returns>In the first string is the file path for the html file. In the second string in the html content that the file should contain.</returns>
        public static Tuple<string, string> GetMarkUpFile(string relativePath, string filePath)
        {
            var mdParser = new Markdown();

            // Store the file's path and contents in these variables initially, and if its a markdown file then reassign after.
            var htmlPath = filePath;
            var htmlText = "";

            // Get the text from the file specified.
            try
            {
                htmlText = string.Concat(from text in Files.File.ReadFile(relativePath + filePath)
                    select text.Text + Environment.NewLine);
            }
            catch(IOException e)
            {
                Report.Error((pub, ex) =>
                {
                    pub.AddWarningDescription("Error in markup file path.");
                    pub.AddReason(e.Message);
                    pub.AddLocation(filePath);
                }, e);
            }

            // Check if a Markdown is specified and if so parse it to get the html. The Path information should specified the 
            // path in the documentation to the file. So change its externsion to html.
            var ext = Path.GetExtension(filePath);
            if (ext != ".md" && ext != ".markdown")
            {
                return new Tuple<string, string>(htmlPath, htmlText);
            }

            htmlPath = filePath.Remove(filePath.LastIndexOf('.')) + ".html";
            htmlText = mdParser.Transform(htmlText);

            return new Tuple<string, string>(htmlPath, htmlText);
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
            string textNowhitespace;
            while (
                (textNowhitespace = lines[lineIndex].Text.Trim()) == ""
                | (textNowhitespace.StartsWith("!") & !textNowhitespace.StartsWith("!>"))
                    ) { lineIndex++; if (lineIndex >= lines.Count) break; }
            return lineIndex == lines.Count;
        }

        public static string NoWhitespace(string text)
        {
            return Regex.Replace(text, @"\s+", "");
        }

        public static List<string> DelimiterExceptBrackets(string text, char delimiter)
        {
            List<string> delimiteredText = new List<string>();
            int withinBracketsDepth = 0;
            int prevIndex = 0, currentIndex = 0;
            foreach (char aChar in text)
            {
                if (aChar == '(' | aChar == '[') { withinBracketsDepth++; }
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
            List<string> delimiteredText = new List<string>();
            bool sQuotes = false;
            bool dQuotes = false;
            int prevIndex = 0, currentIndex = 0;
            foreach (char aChar in text)
            {
                if (!(sQuotes | dQuotes))
                {
                    sQuotes = aChar == '\'';
                    dQuotes = aChar == '"';
                }
                else
                {
                    if (sQuotes) sQuotes = !(aChar == '\'');
                    if (dQuotes) dQuotes = !(aChar == '"');
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
