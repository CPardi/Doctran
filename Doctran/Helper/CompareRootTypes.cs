// <copyright file="CompareRootTypes.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Utilitys;

    public class CompareRootTypes : EqualityComparer<Type>
    {
        private readonly IEnumerable<Type> _types;

        public CompareRootTypes(IEnumerable<Type> types)
        {
            _types = types;
        }

        public override bool Equals(Type x, Type y)
        {
            var xLowestType = x.GetTypeAndBaseTypes().First(t => _types.Contains(t));
            var yLowestType = x.GetTypeAndBaseTypes().First(t => _types.Contains(t));

            return xLowestType == yLowestType;
        }

        public override int GetHashCode(Type obj) => obj?.GetTypeAndBaseTypesAndInterfaces().FirstOrDefault(t => _types.Contains(t))?.GetHashCode() ?? obj?.GetHashCode() ?? 0;
    }
}