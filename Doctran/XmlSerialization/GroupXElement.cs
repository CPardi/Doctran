// <copyright file="GroupXElement.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.XmlSerialization
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;
    using Parsing;

    public class GroupXElement<TParsed> : IGroupXElement
        where TParsed : IFortranObject
    {
        public GroupXElement(string name)
        {
            this.Func = xElements => new XElement(name, xElements);
        }

        public GroupXElement(Func<IEnumerable<XElement>, XElement> func)
        {
            this.Func = func;
        }

        public Type ForType => typeof(TParsed);

        private Func<IEnumerable<XElement>, XElement> Func { get; }

        public XElement Create(IEnumerable<XElement> xElements)
        {
            return this.Func(xElements);
        }
    }
}