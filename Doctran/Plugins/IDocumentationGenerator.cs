﻿// <copyright file="IDocumentationGenerator.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Plugins
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using Helper;
    using Parsing;
    using Parsing.BuiltIn.FortranObjects;

    public interface IDocumentationGenerator
    {
        XElement ParsedSourcesToXml(IFortranObject source);

        XElement HighlightLines(List<FileLine> lines);
    }
}