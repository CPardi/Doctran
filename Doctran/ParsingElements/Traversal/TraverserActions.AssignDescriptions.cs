// <copyright file="TraverserActions.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.ParsingElements.Traversal
{
    using System.Linq;
    using FortranObjects;
    using Parsing;

    public static partial class TraverserActions
    {
        /// <summary>
        ///     Moves named descriptions in to the container or containers with identifier specified by linkedTo.
        /// </summary>
        public static ITraverserAction AssignDescriptions
        {
            get
            {
                return new TraverserAction<NamedDescription>(
                    obj =>
                    {
                        var linkedTo = obj?.LinkedTo;

                        if (linkedTo == null)
                        {
                            return;
                        }

                        // If this is a description directly below the definition statement then dont move it. This is really
                        // just for function where the result name is the same as the function name.
                        if ((obj.Parent as IHasIdentifier)?.Identifier == linkedTo
                            && (obj.Parent as IHasLines)?.Lines.Count > 1 && (obj.Parent as IHasLines)?.Lines[1].Number == obj.Lines[0].Number)
                        {
                            return;
                        }

                        var objsForDescription =
                            obj.Parent.SubObjects
                                .Where(sObjs => (sObjs as IHasIdentifier)?.Identifier == linkedTo);

                        obj.Parent.RemoveSubObject(obj);
                        foreach (var match in objsForDescription.OfType<IContainer>())
                        {
                            match.AddSubObject(obj);
                        }
                    });
            }
        }
    }
}