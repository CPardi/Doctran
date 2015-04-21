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

using Doctran.BaseClasses;

using Doctran.Fbase.Common;


namespace Doctran.Fbase.Files
{
	public class File : XFortranObject
	{
        public String PathAndFilename { get; private set; }
        public FileInfo Info { get; private set; }

        // Reads a file, determines its type and loads the contained procedure and/or modules.
        public File(FortranObject parent, String pathAndFilename)
            : base(parent, "File",File.ReadFile(pathAndFilename), true)
        {
            this.PathAndFilename = pathAndFilename;

            this.Info = new FileInfo(this.PathAndFilename);

            // Get the filename from the inputted string
            Name = Path.GetFileNameWithoutExtension(pathAndFilename);
        }

		public int NumLines{ get { return this.lines.Count - 1; } } // Negate one for the false line added in ReadFile.

        protected override String GetIdentifier() { return this.Name + this.Info.Extension; }

        public static List<FileLine> ReadFile(String pathAndFilename)
        {
            List<FileLine> lines = new List<FileLine>();
            
			// Add a blank line to make the index of the list equal the line number. This also simplifies the <FortranObject>.Seach method.
            lines.Add(new FileLine(0, ""));

            // Open the file at the file path and load into a streamreader. Then, loop through each line and add it to a List.
            using (StreamReader FileReader = new StreamReader(pathAndFilename))
            {
                String Line;
                int lineIndex = 1;
                while ((Line = FileReader.ReadLine()) != null) { lines.Add(new FileLine(lineIndex, Line)); lineIndex++; }
            }

			// Search the source for any include statements and add their content to this file.
            AddIncludedFiles(ref lines, Path.GetDirectoryName(pathAndFilename));

			// Remove any line coninuations by joining onto a single line.
            RemoveContinuationLines(ref lines);
            return lines;
        }

        public static void AddIncludedFiles(ref List<FileLine> lines, String path)
        {
            List<FileLine> ModifyingLines = new List<FileLine>();

            foreach (FileLine line in lines)
            {
                Match matchInclude = Regex.Match(line.Text.Trim(), @"^(?i)#?include\s+['""](.*)['""]");
                if (matchInclude.Success)
                {
                    string includePath = path + Common.Settings.slash + matchInclude.Groups[1].Value.Replace('\\', Common.Settings.slash).Replace('/', Common.Settings.slash);
                    try
                    {
                        ModifyingLines.AddRange(ReadFile(includePath));
                    }
                    catch
                    {
                        Console.WriteLine("Warning: The following file was not found: " + includePath );
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
            while(lineIndex < lines.Count)
            {
                modifyingLines.Add(new FileLine(lineIndex, MergeContinuations(ref lineIndex, lines, false)));
                lineIndex++;
            }
            lines.Clear();
            lines.AddRange(modifyingLines);
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

        /// <summary>
        /// Outputs an XElement.
        /// </summary>
        /// <returns></returns>
        public override XElement XEle()
        {
            this.Info.Refresh();
            XElement xele = base.XEle();
			xele.AddFirst(new XElement("LineCount",this.NumLines),
					 new XElement("Created",this.Info.CreationTime.XEle()),
                     new XElement("LastModified",this.Info.LastWriteTime.XEle()),
                     new XElement("Extension",this.Info.Extension)
                     );

            return xele;
        }
    }
}
