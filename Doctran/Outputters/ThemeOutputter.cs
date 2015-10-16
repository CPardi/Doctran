//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Doctran.Fbase.Outputters
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Common;
    using dotless.Core;
    using MarkdownSharp;

    public class ThemeOutputter
    {
        private readonly Dictionary<string, TextFileCopier> _copiers = new Dictionary<string, TextFileCopier>();

        public ThemeOutputter()
        {
            var lessCopier = new LessFileCopier();
            _copiers.Add(lessCopier.Extension, lessCopier);

            var markdownCopier = new MarkdownFileCopier();
            _copiers.Add(markdownCopier.Extension, markdownCopier);

            var mdCopier = new MdFileCopier();
            _copiers.Add(mdCopier.Extension, mdCopier);

            var xsltIgnorer = new XsltFileIgnorer();
            _copiers.Add(xsltIgnorer.Extension, xsltIgnorer);
        }

        public void Output(Options options)
        {
            var relativeDirectory = Path.GetDirectoryName(options.ProjectFilePath ?? EnvVar.defaultInfoPath) + EnvVar.slash;
            var outputDirectory = options.OutputDirectory + EnvVar.slash;

            // Copy user's extra files.
            foreach (var filePath in options.CopyPaths)
                CopyFile(options.OverwriteExisting, false, outputDirectory, relativeDirectory, relativeDirectory + filePath);

            // Copy and parse user's extra files.
            foreach (var filePath in options.CopyAndParsePaths)
                CopyFile(options.OverwriteExisting, true, outputDirectory, relativeDirectory, relativeDirectory + filePath);

            // Copy theme files.
            CopyFilesFromDir(options.OverwriteExisting, true, true, outputDirectory, EnvVar.ThemeDirectory(options.ThemeName), "");
        }

        private void CopyFile(bool overwrite, bool parse, string outputDirectory, string relativeDirectory, string filePath)
        {
            var extension = Path.GetExtension(filePath);
            if (parse && _copiers.ContainsKey(extension))
            {
                _copiers[extension].Run(relativeDirectory, filePath, outputDirectory, overwrite);
            }
            else
            {
                new FileCopier().Run(relativeDirectory, filePath, outputDirectory, overwrite);
            }
        }

        private void CopyFilesFromDir(bool overwrite, bool parse, bool recursive, string outputDirectory, string relativeDirectory, string directoryPath)
        {
            foreach (var filePath in Directory.GetFiles(relativeDirectory + directoryPath, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
                CopyFile(overwrite, parse, outputDirectory, relativeDirectory, filePath);
        }

        internal class FileCopier
        {
            public virtual void Run(string relativePath, string filePath, string copyToDirectory, bool overwrite)
            {
                var outputFilePath = copyToDirectory + filePath.Substring(relativePath.Length);
                Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(outputFilePath)));
                if (!File.Exists(outputFilePath) || overwrite) File.Copy(filePath, outputFilePath, overwrite);
            }
        }

        internal class LessFileCopier : TextFileCopier
        {
            public LessFileCopier()
                : base(".less", ".css")
            {
            }

            protected override string ReadFile(string filePath)
            {
                return Less.Parse(base.ReadFile(filePath));
            }
        }

        internal class MarkdownFileCopier : MarkdownFileCopierBase
        {
            public MarkdownFileCopier()
                : base(".markdown")
            {
            }
        }

        internal abstract class MarkdownFileCopierBase : TextFileCopier
        {
            private readonly Markdown _parser = new Markdown();

            public MarkdownFileCopierBase(string extension)
                : base(extension, ".html")
            {
            }

            protected override string ReadFile(string filePath)
            {
                return _parser.Transform(base.ReadFile(filePath));
            }
        }

        internal class MdFileCopier : MarkdownFileCopierBase
        {
            public MdFileCopier()
                : base(".md")
            {
            }
        }

        internal abstract class TextFileCopier : FileCopier
        {
            public TextFileCopier(string originalExtension, string newExtension)
            {
                this.Extension = originalExtension;
                this.NewExtension = newExtension;
            }

            public string Extension { get; }
            public string NewExtension { get; }

            public override void Run(string relativePath, string filePath, string copyToDirectory, bool overwrite)
            {
                var outputFilePath = copyToDirectory + filePath.Substring(relativePath.Length);
                Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath));
                var outputPath = ChangeExtension(outputFilePath);
                if (overwrite || !File.Exists(outputPath)) File.WriteAllText(outputPath, this.ReadFile(filePath));
            }

            protected string ChangeExtension(string filePath)
            {
                var actualExtension = Path.GetExtension(filePath);
                if (actualExtension != Extension) throw new FormatException("File path has the extension " + actualExtension + ", expected " + Extension);
                return filePath.Substring(0, filePath.Length - Extension.Length) + NewExtension;
            }

            protected virtual string ReadFile(string filePath)
            {
                using (var fileReader = new StreamReader(filePath))
                {
                    return fileReader.ReadToEnd();
                }
            }
        }

        internal class XsltFileIgnorer : TextFileCopier
        {
            public XsltFileIgnorer()
                : base(".xslt", ".xslt")
            {
            }

            public override void Run(string relativePath, string filePath, string copyToDirectory, bool overwrite)
            {
            }
        }
    }
}