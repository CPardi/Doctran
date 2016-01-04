// <copyright file="SourceFile.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.ParsingElements.FortranObjects
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using ColorCode;
    using ColorCode.Formatting;
    using ColorCode.Styling.StyleSheets;
    using Helper;
    using Parsing;
    using Utilitys;

    public class SourceFile : LinedInternal, IHasName, IHasLines, IHasValidName, ISourceFile
    {
        private readonly FileInfo _info;

        // Reads a file, determines its type and loads the contained procedure and/or modules.
        public SourceFile(string language, string absolutePath, IEnumerable<IContained> subObjects, List<FileLine> originalLines, List<FileLine> lines)
            : base(subObjects, lines)
        {
            this.Name = Path.GetFileName(absolutePath);

            this.AbsolutePath = absolutePath;
            this.OriginalLines = originalLines;
            this.Language = language;
            _info = new FileInfo(this.AbsolutePath);

            // Get the filename from the inputted string
            this.Name = Path.GetFileNameWithoutExtension(absolutePath);
        }

        public override string ObjectName => "Source File";

        public string AbsolutePath { get; }

        public DateTime Created => _info.CreationTime;

        public string Extension => _info.Extension;

        public string Identifier => $"{this.Name}{this.Extension}";

        public string Language { get; }

        public DateTime LastModified => _info.LastWriteTime;

        public int LineCount => this.Lines.Count - 1;

        public string Name { get; }

        public List<FileLine> OriginalLines { get; }

        public string ValidName => StringUtils.ValidName(this.Name);

        public XElement SourceXEle(List<FileLine> lines)
        {
            var str = string.Concat(lines.Select(line => line.Text + Environment.NewLine));

            // Return a syntax highlighted source code.
            var cc = new CodeColorizer();
            var coloredLines = cc.Colorize(
                str,
                Languages.Fortran,
                new HtmlLinedClassFormatter(),
                new LinedStyleSheet())
                .Replace(Environment.NewLine, string.Empty);

            return
                new XElement(
                    "File",
                    new XElement("Name", this.Name),
                    new XElement("Lines", XElement.Parse(coloredLines, LoadOptions.PreserveWhitespace)));
        }
    }
}