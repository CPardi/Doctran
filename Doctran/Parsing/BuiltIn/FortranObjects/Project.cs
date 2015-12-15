// <copyright file="Project.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing.BuiltIn.FortranObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Plugins;
    using Utilitys;

    public class Project : FortranObject
    {
        public Project(IEnumerable<SourceFile> parsedFiles)
            : base(parsedFiles, null)
        {
        }

        public XElement XEle(XElement xmlPassthrough)
        {
            var xele = new XElement("Project");

            xele.Add(xmlPassthrough);
            xele.Add(new XElement("DocCreated", DateTime.Now.ToXElement()));
            xele.Add(
                from info in this.SubObjectsOfType<Description>()
                select new XElement("Description", info.Basic, info.Detailed)
                );

            xele.Add(new XmlGenerator(PluginLoader.AllInterfaceXElements, PluginLoader.AllObjectXElements, PluginLoader.AllObjectGroupXElements).CreateForObject(this));

            return xele;
        }
    }
}