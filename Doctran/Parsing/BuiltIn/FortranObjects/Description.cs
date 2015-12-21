// <copyright file="Description.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing.BuiltIn.FortranObjects
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using Helper;

    public class Description : FortranObject, IDescription
    {
        public Description(XElement basic, XElement detailed, List<FileLine> lines)
            : base(lines)
        {
            this.Basic = basic;
            this.Detailed = detailed;
        }

        public XElement Basic { get; }

        public XElement Detailed { get; }
    }
}