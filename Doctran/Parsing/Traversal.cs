// <copyright file="Traversal.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing
{
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    ///     Provides methods to traverse a project tree.
    /// </summary>
    public static class Traversal
    {
        /// <summary>
        ///     Search the ancestors of <paramref name="contained" /> to check if it is an instance of type
        ///     <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The type to search for.</typeparam>
        /// <param name="contained">The instance to be checked.</param>
        /// <returns>The ancestor of type <typeparamref name="T" />.</returns>
        public static T AncestorOfType<T>(this IContained contained)
            where T : class, IFortranObject
        {
            return SelfOrAncestorOfType<T>(contained.Parent);
        }

        /// <summary>
        ///     Search <paramref name="contained" /> and its the ancestors to check if it is an instance of type
        ///     <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The type to search for.</typeparam>
        /// <param name="contained">The instance to be checked.</param>
        /// <returns>The ancestor of type <typeparamref name="T" />.</returns>
        public static T SelfOrAncestorOfType<T>(this IFortranObject contained)
            where T : class, IFortranObject
        {
            if (contained == null)
            {
                return null;
            }

            IFortranObject current = contained;
            var currentT = contained as T;
            while (current != null && currentT == null)
            {
                current = (current as IContained)?.Parent;
                currentT = current as T;
            }

            return currentT;
        }

        public static ReadOnlyCollection<T> SubObjectsOfType<T>(this IContainer container) => container.SubObjects.OfType<T>().ToList().AsReadOnly();
    }
}