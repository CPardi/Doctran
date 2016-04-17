// <copyright file="MarkdownFileCopier.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Output.Assets
{
    using System.Collections.ObjectModel;
    using MarkdownSharp;
    using Utilitys;

    internal class MarkdownFileCopier : TextFileCopier
    {
        private readonly Markdown _parser = new Markdown();

        public override ReadOnlyCollection<string> FromExtensions => new[] { ".md", ".MD", ".markdown", ".MARKDOWN" }.ToReadOnlyCollection();

        public override string ToExtension => ".html";

        protected override string ReadFile(string filePath)
        {
            return _parser.Transform(base.ReadFile(filePath));
        }
    }
}