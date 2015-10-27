﻿//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Doctran.Parsing.FortranObjects
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;
    using ColorCode;
    using Helper;
    using Output;
    using Parsing;
    using Reporting;
    using Utilitys;

    public class File : XFortranObject
    {
        #region Constructor

        // Reads a file, determines its type and loads the contained procedure and/or modules.
        public File(string pathAndFilename, IEnumerable<FortranObject> subObjects, List<FileLine> lines, IEnumerable<ObjectGroup> objectGroups)
            : base(Path.GetFileName(pathAndFilename), subObjects,"File", lines)
        {
            this.PathAndFilename = pathAndFilename;
            ObjectGroups = objectGroups;
            this.Info = new FileInfo(this.PathAndFilename);

            // Get the filename from the inputted string
            Name = Path.GetFileNameWithoutExtension(pathAndFilename);
        }

        #endregion

        #region Public Properties

        public string PathAndFilename { get; private set; }
        public IEnumerable<ObjectGroup> ObjectGroups { get; }
        public FileInfo Info { get; private set; }
        public List<FileLine> OriginalLines { get; set; }
        public int NumLines { get { return this.lines.Count - 1; } } // Negate one for the false line added in ReadFile.

        #endregion

        #region Public Methods

        public XElement SourceXEle
        {
            get
            {
                var str = string.Concat(this.OriginalLines.Select(line => line.Text + Environment.NewLine));

                // Return a syntax highlighted source code.
                CodeColorizer cc = new CodeColorizer();
                return
                    new XElement("File",
                    new XElement("Name", this.Name),
                    new XElement("Lines",
                    XElement.Parse(cc.Colorize(str, Languages.Fortran, new ColorCode.Formatting.HtmlLinedClassFormatter(), new ColorCode.Styling.StyleSheets.LinedStyleSheet()).Replace(Environment.NewLine, ""), LoadOptions.PreserveWhitespace)
                    ));
            }
        }

        public static List<FileLine> ReadFile(string pathAndFilename)
        {
            List<FileLine> lines = new List<FileLine>();

            try
            {
                // Open the file at the file path and load into a streamreader. Then, loop through each line and add it to a List.
                using (StreamReader fileReader = new StreamReader(pathAndFilename))
                {
                    string line;
                    int lineIndex = 1;
                    while ((line = fileReader.ReadLine()) != null)
                    {
                        CheckForPreprocessing(Path.GetFileName(pathAndFilename), line);
                        lines.Add(new FileLine(lineIndex, line));
                        lineIndex++;
                    }
                }
            }
            catch (IOException e)
            {
                Report.Warning(
                    pub =>
                    {
                        pub.AddErrorDescription("Could not read file.");
                        pub.AddReason(e.Message);
                    });

                throw;
            }

            return lines;
        }

        public static List<FileLine> PreProcessFile(string pathAndFilename, List<FileLine> lines)
        {
            List<FileLine> modLines = new List<FileLine>();

            modLines.AddRange(lines);

            // Search the source for any include statements and add their content to this file.
            AddIncludedFiles(ref modLines, Path.GetDirectoryName(pathAndFilename));

            // Remove any line coninuations by joining onto a single line.
            RemoveContinuationLines(ref modLines);

            // Add a blank line to simplify the <FortranObject>.Seach method.
            modLines.Insert(0, new FileLine(0, ""));

            return modLines;
        }

        public static void AddIncludedFiles(ref List<FileLine> lines, string path)
        {
            List<FileLine> modifyingLines = new List<FileLine>();

            foreach (FileLine line in lines)
            {
                CheckForPreprocessing(Path.GetFileName(path), line.Text);
                Match matchInclude = Regex.Match(line.Text.Trim(), @"^(?i)include\s+['""](.*)['""]");
                if (matchInclude.Success)
                {
                    string includePath = path + EnvVar.Slash + matchInclude.Groups[1].Value.Replace('\\', EnvVar.Slash).Replace('/', EnvVar.Slash);
                    try
                    {
                        modifyingLines.AddRange(ReadFile(includePath).Select(l => new FileLine(line.Number, l.Text)));
                    }
                    catch
                    {
                        Console.WriteLine("Warning: The following file was not found: " + includePath);
                    }
                }
                else
                {
                    modifyingLines.Add(line);
                }
            }
            lines.Clear();
            lines.AddRange(modifyingLines);
        }

        public static void RemoveContinuationLines(ref List<FileLine> lines)
        {
            List<FileLine> modifyingLines = new List<FileLine>();

            int lineIndex = 0;
            while (lineIndex < lines.Count)
            {
                int num = lines[lineIndex].Number;
                modifyingLines.AddRange(
                    from l in SplitEndings(MergeContinuations(ref lineIndex, lines, false))
                    select new FileLine(num, l));
                lineIndex++;
            }
            lines.Clear();
            lines.AddRange(modifyingLines);
        }

        #endregion

        #region Private Methods

        private static void CheckForPreprocessing(string filename, string line)
        {
            if (Regex.IsMatch(HelperUtils.RemoveInlineComment(line), @"^\s*(?:#define|#elif|#elifdef|#elifndef|#else|#endif|#error|#if|#ifdef|#ifndef|#line|#pragma|#undef|#include)"))
            {
                Report.Error(
                    (pub, ex) =>
                    {
                        pub.AddErrorDescription("Source contains preprocessor directives, please pass the code through your preprocessor and rerun Doctran upon the output.");
                    }, new Exception("Source contains preprocessor directives."));
            }
        }

        private static List<string> SplitEndings(string line)
        {
            string[] lineNocomm = line.Split('!');

            List<string> lines = HelperUtils.DelimiterExceptQuotes(lineNocomm[0].Trim(), ';');
            if (lines.Count > 0)
            {
                lines[lines.Count - 1] += (lineNocomm.Length > 1 ? "!" + string.Concat(lineNocomm.Skip(1)) : "");
                return lines;
            }
            else
                return new List<string>() { line };
        }

        private static string MergeContinuations(ref int lineIndex, List<FileLine> lines, bool removeComment)
        {
            string lineTextNoComment = HelperUtils.RemoveInlineComment(lines[lineIndex].Text).TrimStart('&');
            if (lineTextNoComment.EndsWith("&"))
            {
                lineIndex++;
                HelperUtils.SkipComment(lines, ref lineIndex);
                if (HelperUtils.RemoveInlineComment(lines[lineIndex].Text) == "") lineIndex++;
                return lineTextNoComment.TrimEnd('&') + MergeContinuations(ref lineIndex, lines, true);
            }
            else if (removeComment)
            {
                return lineTextNoComment.TrimEnd('&');
            }
            else
            {
                return lines[lineIndex].Text;
            }
        }

        #endregion

        #region Overrides

        protected override string GetIdentifier()
        {
            return this.Name + this.Info.Extension;
        }

        /// <summary>
        /// Outputs an XElement.
        /// </summary>
        /// <returns></returns>
        public override XElement XEle()
        {
            this.Info.Refresh();
            XElement xele = base.XEle();
            xele.AddFirst(new XElement("LineCount", this.NumLines),
                     new XElement("Created", this.Info.CreationTime.XEle()),
                     new XElement("LastModified", this.Info.LastWriteTime.XEle()),
                     new XElement("ValidName", HelperUtils.ValidName(this.Name)),
                     new XElement("Extension", this.Info.Extension)
                     );

            return xele;
        }

        #endregion
    }
}