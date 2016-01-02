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
    using Parsing;

    public static class CollectionUtils
    {
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

        public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> @this)
        {
            return @this.ToList().AsReadOnly();
        }

        public static void AddSubObject(IContainer container, IContained contained)
        {
            contained.Parent = container;
            container.SubObjects.Add(contained);
        }

        public static void AddSubObjects(IContainer container, IEnumerable<IContained> containedItems)
        {
            var containedItemList = containedItems as IList<IContained> ?? containedItems.ToList();
            foreach (var item in containedItemList)
            {
                item.Parent = container;
            }

            container.SubObjects.AddRange(containedItemList);
        }

        public static void RemoveSubObject(IContainer container, IContained contained)
        {
            container.SubObjects.Remove(contained);
        }

        public static void RemoveSubObjects(IContainer container, IEnumerable<IContained> containeItems)
        {
            foreach (var item in containeItems)
            {
                container.RemoveSubObject(item);
            }
        }
    }
}