// <copyright file="XmlGeneratorKey.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.XmlSerialization
{
    using System;

    internal class XmlGeneratorKey
    {
        public XmlGeneratorKey(Type type, XmlTraversalType xmlTraversalType)
        {
            this.Type = type;
            this.XmlTraversalType = xmlTraversalType;
        }

        public Type Type { get; }

        public XmlTraversalType XmlTraversalType { get; }
    }
}