//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using ColorCode;

using Doctran.BaseClasses;

using Doctran.Fbase.Common;


namespace Doctran.Fbase.Files
{
    public class File : XFortranObject
    {
        #region Constructor

        // Reads a file, determines its type and loads the contained procedure and/or modules.
        public File(FortranObject parent, String pathAndFilename, List<FileLine> lines)
            : base(parent, Path.GetFileName(pathAndFilename), "File", File.PreProcessFile(pathAndFilename, lines), true)
        {
            this.PathAndFilename = pathAndFilename;
            this.OriginalLines = lines;
            this.Info = new FileInfo(this.PathAndFilename);

            // Get the filename from the inputted string
            Name = Path.GetFileNameWithoutExtension(pathAndFilename);
        }

        #endregion

        #region Public Properties

        public String PathAndFilename { get; private set; }
        public FileInfo Info { get; private set; }
        public List<FileLine> OriginalLines { get; set; }
        public int NumLines { get { return this.lines.Count - 1; } } // Negate one for the false line added in ReadFile.

        #endregion

        #region Public Methods

        public XElement SourceXEle
        {
            get
            {
                // Get original source. Skip the first line added for convenience.
                var str = String.Concat(this.OriginalLines.Skip(1).Select(line => line.Text + Environment.NewLine));

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

        public static List<FileLine> ReadFile(String pathAndFilename)
        {
            List<FileLine> lines = new List<FileLine>();

            // Add a blank line to make the index of the list equal the line number. This also simplifies the <FortranObject>.Seach method.
            lines.Add(new FileLine(0, ""));

            try
            {
                // Open the file at the file path and load into a streamreader. Then, loop through each line and add it to a List.
                using (StreamReader FileReader = new StreamReader(pathAndFilename))
                {
                    String line;
                    int lineIndex = 1;
                    while ((line = FileReader.ReadLine()) != null) { CheckForPreprocessing(Path.GetFileName(pathAndFilename), line); lines.Add(new FileLine(lineIndex, line)); lineIndex++; }
                }
            }
            catch (IOException) { throw; }

            return lines;
        }

        public static List<FileLine> PreProcessFile(String pathAndFilename, List<FileLine> lines)
        {
            List<FileLine> modLines = new List<FileLine>();

            modLines.AddRange(lines);

            // Search the source for any include statements and add their content to this file.
            AddIncludedFiles(ref modLines, Path.GetDirectoryName(pathAndFilename));

            // Remove any line coninuations by joining onto a single line.
            RemoveContinuationLines(ref modLines);
            return modLines;
        }

        public static void AddIncludedFiles(ref List<FileLine> lines, String path)
        {
            List<FileLine> ModifyingLines = new List<FileLine>();

            foreach (FileLine line in lines)
            {
                CheckForPreprocessing(Path.GetFileName(path), line.Text);
                Match matchInclude = Regex.Match(line.Text.Trim(), @"^(?i)include\s+['""](.*)['""]");
                if (matchInclude.Success)
                {
                    string includePath = path + Common.Settings.slash + matchInclude.Groups[1].Value.Replace('\\', Common.Settings.slash).Replace('/', Common.Settings.slash);
                    try
                    {
                        ModifyingLines.AddRange(ReadFile(includePath).Select(l => new FileLine(line.Number, l.Text)));
                    }
                    catch
                    {
                        Console.WriteLine("Warning: The following file was not found: " + includePath);
                    }
                }
                else
                {
                    ModifyingLines.Add(line);
                }
            }
            lines.Clear();
            lines.AddRange(ModifyingLines);
        }

        public static void RemoveContinuationLines(ref List<FileLine> lines)
        {
            List<FileLine> modifyingLines = new List<FileLine>();

            int lineIndex = 0;
            while (lineIndex < lines.Count)
            {
                modifyingLines.Add(new FileLine(lines[lineIndex].Number, MergeContinuations(ref lineIndex, lines, false)));
                lineIndex++;
            }
            lines.Clear();
            lines.AddRange(modifyingLines);
        }

        #endregion

        #region Private Methods

        private static void CheckForPreprocessing(String filename, String line)
        {
            if (Regex.IsMatch(Helper.RemoveInlineComment(line), @"^\s*(?:#define|#elif|#elifdef|#elifndef|#else|#endif|#error|#if|#ifdef|#ifndef|#line|#pragma|#undef|#include)"))
            {
                UserInformer.GiveError(filename, "preprocessor directives detected, please pass the code through your preprocessor and rerun Doctran upon the output.");
            }
        }

        private static String MergeContinuations(ref int lineIndex, List<FileLine> lines, bool removeComment)
        {
            String lineText_noComment = lines[lineIndex].Text.Split('!')[0].Trim().TrimStart('&');
            if (lineText_noComment.EndsWith("&"))
            {
                lineIndex++;
                Helper.SkipComment(lines, ref lineIndex);
                return lineText_noComment.TrimEnd('&') + MergeContinuations(ref lineIndex, lines, true);
            }
            else if (removeComment)
            {
                return lineText_noComment.TrimEnd('&');
            }
            else
            {
                return lines[lineIndex].Text;
            }
        }

        #endregion

        #region Overrides

        protected override String GetIdentifier()
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
                     new XElement("Extension", this.Info.Extension)
                     );

            return xele;
        }

        #endregion
    }
}
