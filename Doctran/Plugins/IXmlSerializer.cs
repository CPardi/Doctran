// <copyright file="IXmlSerializer.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Plugins
{
    using System.Xml.Linq;
    using Parsing;

    public interface IDocumentationDefinition
    {
        string[] ThemePartNames { get; }

        XElement HighlightLines(string source);

        XElement ParsedSourcesToXml(IFortranObject source);
    }
}