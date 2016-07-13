// <copyright file="ObjectXPassthrough.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.XmlSerialization
{
    using System;
    using Parsing;

    public class ObjectXPassthrough<TParsed> : IObjectXBase
        where TParsed : IFortranObject
    {
        public ObjectXPassthrough(XmlTraversalType xmlTraversalType = XmlTraversalType.HeadOrSubObjects)
        {
            this.XmlTraversalType = xmlTraversalType;
        }

        public Type ForType => typeof(TParsed);

        public XmlTraversalType XmlTraversalType { get; }
    }
}