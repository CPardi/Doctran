// <copyright file="TraverserActions.CheckValidity.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.ParsingElements.Traversal
{
    using System;
    using Parsing;

    public static partial class TraverserActions
    {
        /// <summary>
        ///     Check instances of <typeparamref name="T" /> for their validity as defined by <paramref name="isValid" />.
        /// </summary>
        /// <typeparam name="T">The type of instances to check.</typeparam>
        /// <param name="isValid">A function that returns true if valid and false otherwise.</param>
        /// <param name="getErrorDetail">A function returning a string describing the detail of the error.</param>
        /// <returns>An <see cref="ITraverserAction" /> for use with <see cref="Traverser" />.</returns>
        public static ITraverserAction CheckValidity<T>(Func<T, bool> isValid, Func<T, string> getErrorDetail)
            where T : IFortranObject
        {
            return new TraverserAction<T>(
                (obj, errLis) =>
                {
                    if (isValid(obj))
                    {
                        return;
                    }

                    // If contained within another, then remove the offending object.
                    var contained = obj as IContained;
                    contained?.Parent?.RemoveSubObject(contained);

                    // Report the error.
                    errLis.Error(new TraverserException(obj, $"A '{Names.OfType(obj.GetType()).ToLower()}' contains an invalid value and has been ignored. {getErrorDetail(obj)}"));
                });
        }
    }
}