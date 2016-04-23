// <copyright file="Traversal.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Utilitys;

    /// <summary>
    /// Provides methods to traverse a project tree.
    /// </summary>
    public static class Traversal
    {
        /// <summary>
        /// Search the ancestors of an instance of <see cref="IContained"/> for an instance of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to search for.</typeparam>
        /// <param name="contained">The instances whose ancestors are to be searched.</param>
        /// <returns>The ancestor of type <typeparamref name="T"/>.</returns>
        public static T AncestorOfType<T>(this IContained contained)
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