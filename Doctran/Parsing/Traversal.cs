// <copyright file="Traversal.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Helper;
    using ParsingElements;
    using ParsingElements.FortranObjects;

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

        public static IEnumerable<T> Find<T>(this Project project, IIdentifier identifier)
            where T : IHasIdentifier
        {
            var result = new List<T>();
            Action<T, IErrorListener<TraverserException>> subObjectSearch = (o, e) =>
            {
                if (Equals(o.Identifier, identifier))
                {
                    result.Add(o);
                }
            };

            Action<IQuasiContainer, IErrorListener<TraverserException>> quasiObjectSearch = (qo, e) =>
            {
                try
                {
                    result.AddRange(qo.QuasiObjects.OfType<T>().Where(o => Equals(o.Identifier, identifier)));
                }
                catch (TraverserException)
                {
                    // Ignore any error if searching.
                }
            };

            new Traverser(
                "Find",
                new TraverserAction<T>(subObjectSearch),
                new TraverserAction<IQuasiContainer>(quasiObjectSearch))
                .Go(project);

            return result;
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

            var current = contained;
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