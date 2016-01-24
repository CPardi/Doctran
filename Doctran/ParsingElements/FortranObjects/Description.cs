// <copyright file="Description.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.ParsingElements.FortranObjects
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using Helper;
    using Parsing;

    public class Description : IDescription
    {
        public Description(XElement basic, XElement detailed, List<FileLine> lines)
        {
            this.Basic = basic;
            this.Detailed = detailed;
            this.Lines = lines;
        }

        public XElement Basic { get; }

        public XElement Detailed { get; }

        public List<FileLine> Lines { get; }

        public IContainer Parent { get; set; }

        public string ObjectName => Names.OfType(this.GetType());
    }
}