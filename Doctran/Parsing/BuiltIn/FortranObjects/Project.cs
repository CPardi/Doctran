﻿// <copyright file="Project.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing.BuiltIn.FortranObjects
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Xml.Linq;
    using Plugins;
    using Utilitys;

    public class Project : FortranObject
    {
        public Project(IEnumerable<ISourceFile> parsedFiles)
            : base()
        {
            var parsedFilesList = parsedFiles as IList<ISourceFile> ?? parsedFiles.ToList();
            this.Sources = parsedFilesList.ToList().AsReadOnly();
            this.AddSubObjects(parsedFilesList);
        }

        public ReadOnlyCollection<ISourceFile> Sources { get; }

        public XElement XEle(XElement xmlPassthrough)
        {
            var xEle = new XElement("Project");

            xEle.Add(xmlPassthrough);
            xEle.Add(new XElement("DocCreated", DateTime.Now.ToXElement()));
            xEle.Add(
                from info in this.SubObjectsOfType<Description>()
                select new XElement("Description", info.Basic, info.Detailed)
                );

            var sourceXEle = new XElement(this.SourcesXmlHead);
            foreach (var source in this.Sources)
            {
                var generator = DocumentationManager.TryGetDefinitionByIdentifier(source.Language);
                sourceXEle.Add(generator.ParsedSourcesToXml(source));
            }

            xEle.Add(sourceXEle);
            return xEle;
        }

        public XName SourcesXmlHead => "Files";
    }
}