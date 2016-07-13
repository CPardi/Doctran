﻿// <copyright file="Program.CreateSourceXml.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran
{
    using System.Linq;
    using System.Xml.Linq;
    using ParsingElements.FortranObjects;
    using Plugins;

    public partial class Program
    {
        private static XDocument CreateSourceXml(Project project)
        {
            var xElements =
                from source in project.Sources
                let highlighter = DocumentationManager.TryGetDefinitionByIdentifier(source.Language)
                select new XElement(
                    "File",
                    new XElement("Identifier", source.Identifier),
                    highlighter.HighlightLines(source.OriginalLines));
            var sourceDocument = new XDocument(new XElement("Source", xElements));

            return sourceDocument;
        }
    }
}