// <copyright file="OtherUtils.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Utilitys
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Helper;
    using MarkdownSharp;
    using Reporting;

    public static class OtherUtils
    {
        public static List<FileLine> ReadFile(string path)
        {
            var lines = new List<FileLine>();

            try
            {
                // Open the file at the file path and load into a streamreader. Then, loop through each line and add it to a List.
                using (var fileReader = new StreamReader(path))
                {
                    string line;
                    var lineIndex = 1;
                    while ((line = fileReader.ReadLine()) != null)
                    {
                        lines.Add(new FileLine(lineIndex, line));
                        lineIndex++;
                    }
                }
            }
            catch (IOException e)
            {
                Report.Error(pub => pub.DescriptionReason(ReportGenre.FileRead, e.Message), e);
            }

            return lines;
        }

        public static void ConsoleGotoNewLine()
        {
            if (Console.CursorLeft != 0)
            {
                Console.WriteLine(string.Empty);
            }
        }

        public static void CreateDirectory(string relativePath)
        {
            if (!Directory.Exists(Path.GetFullPath(relativePath)))
            {
                Directory.CreateDirectory(Path.GetFullPath(relativePath));
            }
        }

        /// <summary>
        ///     Return a html file path and html content for a html or markdown file.
        /// </summary>
        /// <param name="filePath">The path to the html or markdown file.</param>
        /// <returns>
        ///     In the first string is the file path for the html file. In the second string in the html content that the file
        ///     should contain.
        /// </returns>
        public static Tuple<string, string> GetMarkUpFile(string relativePath, string filePath)
        {
            var mdParser = new Markdown();

            // Store the file's path and contents in these variables initially, and if its a markdown file then reassign after.
            var htmlPath = filePath;
            var htmlText = "";

            // Get the text from the file specified.
            try
            {
                htmlText = string.Concat(from text in ReadFile(relativePath + filePath)
                    select text.Text + Environment.NewLine);
            }
            catch (IOException e)
            {
                Report.Error(pub => pub.DescriptionReasonLocation(ReportGenre.FileRead, $"Error in markup file path. {e.Message}", filePath), e);
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

        /// <summary>
        ///     Moves to the next line in the file which is not white space nor a fortran Description.
        /// </summary>
        public static bool GotoNextUsefulLine(List<FileLine> lines, ref int lineIndex)
        {
            if (lineIndex >= lines.Count)
            {
                return true;
            }
            string textNowhitespace;
            while (
                (textNowhitespace = lines[lineIndex].Text.Trim()) == ""
                | (textNowhitespace.StartsWith("!") & !textNowhitespace.StartsWith("!>"))
                )
            {
                lineIndex++;
                if (lineIndex >= lines.Count)
                {
                    break;
                }
            }
            return lineIndex == lines.Count;
        }

        public static string RegexMultipleOr(IEnumerable<string> regexes)
        {
            var regexArray = regexes as string[] ?? regexes.ToArray();
            return regexArray.First() + string.Concat(regexArray.Skip(1).Select(d => "|" + d));
        }

        /// <summary>
        ///     Moves to the next code line. This means comments and doctran comments are skipped.
        /// </summary>
        public static void SkipComment(List<FileLine> lines, ref int lineNumber)
        {
            while (lines[lineNumber].Text.Trim().StartsWith("!"))
            {
                lineNumber++;
            }
        }

        public static void Stop()
        {
            Environment.Exit(1);
        }
    }
}