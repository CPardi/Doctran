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
    using Helper;
    using Parsing;
    using Scope;
    using Utilitys;

    public class SourceFile : LinedInternal, ISourceFile
    {
        private readonly FileInfo _info;

        // Reads a file, determines its type and loads the contained procedure and/or modules.
        public SourceFile(string language, string absolutePath, IEnumerable<IContained> subObjects, string originalLines, List<FileLine> lines)
            : base(subObjects, lines)
        {
            this.AbsolutePath = absolutePath;
            this.OriginalLines = originalLines;
            this.Language = language;
            _info = new FileInfo(this.AbsolutePath);
        }

        public string AbsolutePath { get; }

        public DateTime Created => _info.CreationTime;

        public string Extension => _info.Extension;

        public string Identifier => this.Name;

        public string Language { get; }

        public DateTime LastModified => _info.LastWriteTime;

        public int LineCount => this.Lines.Count - 1;

        public string Name => PathUtils.FilenameAndAncestorDirectories(this.AbsolutePath, this.NameUniquenessLevel);

        public int NameUniquenessLevel { get; set; } = 0;

        public string OriginalLines { get; }

        public string ValidName => this.Name.Replace("/", "_newdir_").Replace("\\", "_newdir_");
    }
}