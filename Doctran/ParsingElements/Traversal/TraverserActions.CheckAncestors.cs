// <copyright file="TraverserActions.CheckAncestors.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.ParsingElements.Traversal
{
    using System;
    using System.Linq;
    using Parsing;
    using Utilitys;

    public static partial class TraverserActions
    {
        /// <summary>
        ///     Returns a <see cref="ITraverserAction" /> action that ensures instances of type
        ///     <typeparamref name="T" /> have ancestors within the type list <paramref name="validAncestorTypes" />.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <param name="validAncestorTypes">An array of the types that may be ancestors of that object.</param>
        /// <returns>An <see cref="ITraverserAction{T}" />.</returns>
        public static ITraverserAction CheckAncestors<T>(params Type[][] validAncestorTypes)
            where T : IContained
        {
            return new TraverserAction<T>(
                (obj, errLis) =>
                {
                    IContained current = obj;
                    foreach (var parentTypes in validAncestorTypes)
                    {
                        CheckInstanceParent(parentTypes, current, errLis);
                        current = current.Parent as IContained;

                        if (current != null)
                        {
                            continue;
                        }

                        var parentsText = parentTypes.Select(t => $"'{Names.OfType(t).ToLower()}s'").DelimiteredConcat(", ", " or ");
                        var message = $"A '{obj.ObjectName.ToLower()}' does not have a parent. " +
                          $"'{obj.ObjectName.ToUpperFirstLowerRest()}s' are expected to have {parentsText} as parents.";
                        errLis.Error(new TraverserException(obj, message));
                        break;
                    }
                });
        }
    }
}