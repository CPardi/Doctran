// <copyright file="KeyComparer.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.XmlSerialization
{
    using System;
    using System.Collections.Generic;
    using Helper;

    internal class KeyComparer : EqualityComparer<XmlGeneratorKey>
    {
        private readonly CompareRootTypes _compareRootTypes;

        public KeyComparer(IEnumerable<Type> types)
        {
            _compareRootTypes = new CompareRootTypes(types);
        }

        public override bool Equals(XmlGeneratorKey x, XmlGeneratorKey y)
        {
            return Equals(x.XmlTraversalType, y.XmlTraversalType) && _compareRootTypes.Equals(x.Type, y.Type);
        }

        public override int GetHashCode(XmlGeneratorKey obj)
        {
            if (obj == null)
            {
                return 0;
            }

            unchecked
            {
                var hash = 17;
                hash = (hash * 31) + _compareRootTypes.GetHashCode(obj.Type);
                hash = (hash * 31) + obj.XmlTraversalType.GetHashCode();
                return hash;
            }
        }
    }
}