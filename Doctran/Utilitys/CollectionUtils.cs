// <copyright file="CollectionUtils.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Utilitys
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public static class CollectionUtils
    {
        public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> @this)
        {
            return @this.ToList().AsReadOnly();
        }

        public static IEnumerable<T> Empty<T>()
        {
            return new List<T>();
        }

        public static IEnumerable<T> Repeat<T>(int times, T tIn)
        {
            for (var index = 0; index < times; index++)
            {
                yield return tIn;
            }
        }

        public static IEnumerable<T> Singlet<T>(T tInstance)
        {
            yield return tInstance;
        }
    }
}