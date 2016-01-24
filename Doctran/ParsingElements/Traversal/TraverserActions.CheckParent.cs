// <copyright file="TraverserActions.CheckParent.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.ParsingElements.Traversal
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Parsing;
    using Utilitys;

    public static partial class TraverserActions
    {
        /// <summary>
        ///     Returns a <see cref="ITraverserAction" /> action that ensures instances of type
        ///     <typeparamref name="T" /> only have a parent within the type list <paramref name="validParentTypes" />.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <param name="validParentTypes">An array of the types that may be parents of that object.</param>
        /// <returns>An <see cref="ITraverserAction{T}" />.</returns>
        public static ITraverserAction CheckParent<T>(params Type[] validParentTypes)
            where T : IContained
        {
            return new TraverserAction<T>(
                (obj, errLis) =>
                {
                    foreach (var type in validParentTypes)
                    {
                        Debug.Assert(type.GetInterface(nameof(IFortranObject)) != null, $"The types given within '{validParentTypes}' must implement the interface {nameof(IContained)}");
                    }

                    var valid = false;
                    obj.Parent.GetType().ForTypeAndInterfaces(t => valid |= validParentTypes.Contains(t));

                    if (valid)
                    {
                        return;
                    }

                    var parentsText = validParentTypes.Select(t => $"'{Names.OfType(t).ToLower()}s'").DelimiteredConcat(", ", " or ");
                    var message = $"A '{obj.ObjectName.ToLower()}' has a '{obj.Parent.ObjectName.ToLower()}' for a parent. " +
                                  $"'{obj.ObjectName.ToUpperFirstLowerRest()}s' are expected to have {parentsText} as parents.";
                    errLis.Error(new TraverserException(obj, message));
                });
        }
    }
}