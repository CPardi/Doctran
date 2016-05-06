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
        /// <summary>
        /// Return the differnce between two enumerations.
        /// </summary>
        /// <typeparam name="T">The type of objects in the enumerations.</typeparam>
        /// <param name="this">A list of objects.</param>
        /// <param name="that">A list of objects to remove from from <paramref name="this"/>.</param>
        /// <returns>The list <paramref name="this"/> without items appearing in <paramref name="that"/>.</returns>
        public static IEnumerable<T> Difference<T>(this IEnumerable<T> @this, IEnumerable<T> that)
        {
            return @this.Where(item => !that.Contains(item));
        }

        public static void AddSubObject(List<IContained> subObjects, IContainer container, IContained contained)
        {
            contained.Parent = container;
            subObjects.Add(contained);
        }

        public static void AddSubObjects(List<IContained> subObjects, IContainer container, IEnumerable<IContained> containedItems)
        {
            var containedItemList = containedItems as IList<IContained> ?? containedItems.ToList();
            foreach (var item in containedItemList)
            {
                item.Parent = container;
            }

            subObjects.AddRange(containedItemList);
        }

        public static IEnumerable<T> Empty<T>()
        {
            return new List<T>();
        }

        public static void InsertSubObject(List<IContained> subObjects, IContainer container, int index, IContained contained)
        {
            contained.Parent = container;
            subObjects.Insert(index, contained);
        }

        public static IEnumerable<TIn> NotOfType<TIn, TNot>(this IEnumerable<TIn> @this)
            where TNot : TIn => @this.Where(item => !(item is TNot));

        /// <summary>
        ///     Removes all matching items from a list.
        /// </summary>
        /// <typeparam name="T">The list item type.</typeparam>
        /// <param name="this">The list to remove from.</param>
        /// <param name="items">The items to remove if found within list.</param>
        /// <returns>The number of items removed.</returns>
        public static int RemoveAll<T>(this List<T> @this, IEnumerable<T> items)
        {
            return @this.RemoveAll(items.Contains);
        }

        public static void RemoveSubObject(List<IContained> subObjects, IContainer container, IContained contained)
        {
            subObjects.Remove(contained);
        }

        public static void RemoveSubObjects(IContainer container, IEnumerable<IContained> containeItems)
        {
            foreach (var item in containeItems)
            {
                container.RemoveSubObject(item);
            }
        }

        public static IEnumerable<T> Repeat<T>(int times, T tIn)
        {
            for (var index = 0; index < times; index++)
            {
                yield return tIn;
            }
        }

        public static IEnumerable<T> Singlet<T>(this T @this)
        {
            yield return @this;
        }

        public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> @this)
        {
            return @this.ToList().AsReadOnly();
        }
    }
}