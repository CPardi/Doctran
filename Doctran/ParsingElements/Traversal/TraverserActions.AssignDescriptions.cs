//// <copyright file="TraverserActions.AssignDescriptions.cs" company="Christopher Pardi">
////     Copyright © 2015 Christopher Pardi
////     This Source Code Form is subject to the terms of the Mozilla Public
////     License, v. 2.0. If a copy of the MPL was not distributed with this
////     file, You can obtain one at http://mozilla.org/MPL/2.0/.
//// </copyright>

//namespace Doctran.ParsingElements.Traversal
//{
//    using System.Linq;
//    using FortranObjects;
//    using Parsing;
//    using Scope;

//    /// <summary>
//    /// Contains a number of <see cref="ITraverserAction"/> for use with <see cref="Traverser"/>.
//    /// </summary>
//    public static partial class TraverserActions
//    {
//        /// <summary>
//        ///     Gets an <see cref="ITraverserAction"/> that moves named descriptions in to the container or containers with identifier specified by linkedTo.
//        /// </summary>
//        public static ITraverserAction AssignDescriptions
//        {
//            get
//            {
//                return new TraverserAction<NamedDescription>(
//                    (obj, errLis) =>
//                    {
//                        var linkedTo = obj?.LinkedTo;

//                        if (linkedTo == null)
//                        {
//                            return;
//                        }

//                        // If this is a description directly below the definition statement then dont move it. This is really
//                        // just for function where the result name is the same as the function name.
//                        if (Equals((obj.Parent as IHasIdentifier)?.Identifier, linkedTo)
//                            && (obj.Parent as IHasLines)?.Lines?.Count > 1 && (obj.Parent as IHasLines)?.Lines[1].Number == obj.Lines[0].Number)
//                        {
//                            return;
//                        }

//                        // Try and find the linked identifier in the scope of the description.
//                        var scope = obj.SelfOrAncestorOfType<IHasScope>().Scope;
//                        if (!scope.Exists<IHasIdentifier>(linkedTo))
//                        {
//                            errLis.Error(new TraverserException(obj, $"Named description was ignored, as the identifier '{linkedTo}' was not found in scope."));
//                            obj.Parent.RemoveSubObject(obj);
//                            return;
//                        }

//                        var objsForDescription = scope.GetObjectByIdentifier<IHasIdentifier>(linkedTo);

//                        obj.Parent.RemoveSubObject(obj);
//                        (objsForDescription as IContainer)?.AddSubObject(new Description(obj.Basic, obj.Detailed, obj.Lines));
//                    });
//            }
//        }
//    }
//}