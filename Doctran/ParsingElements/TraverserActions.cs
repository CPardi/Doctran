// <copyright file="TraverserActions.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing.BuiltIn
{
    using System.Linq;
    using FortranObjects;
    using Reporting;

    public static class TraverserActions
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
                            && obj.Parent.Lines.Count > 1 && obj.Parent.Lines[1].Number == obj.Lines[0].Number)
                        {
                            return;
                        }

                        var objsForDescription =
                            obj.Parent.SubObjects
                                .Where(sObjs => (sObjs as IHasIdentifier)?.Identifier == linkedTo);

                        obj.Parent.SubObjects.Remove(obj);
                        foreach (var match in objsForDescription)
                        {
                            match.AddSubObject(obj);
                        }
                    });
            }
        }

        public static ITraverserAction CheckDescriptionLinkage
        {
            get
            {
                return new TraverserAction<NamedDescription>(
                    obj =>
                    {
                        // Return if OK.
                        if ((obj.Parent as IHasIdentifier)?.Identifier == obj.LinkedTo)
                        {
                            return;
                        }

                        // Throw exception if not.
                        obj.Parent.SubObjects.Remove(obj);
                        throw new TraverserException(obj, "Description meta-data was ignored. Description identifier does not match parent identifier.");
                    });
            }
        }

        public static ITraverserAction CheckDescriptionUniqueness
        {
            get
            {
                return new TraverserAction<IDescription>(
                    obj =>
                    {
                        // Return if OK.
                        if (obj.Parent.SubObjects.OfType<IDescription>().Count() <= 1)
                        {
                            return;
                        }

                        // Throw exception if not.
                        obj.Parent.SubObjects.Remove(obj);
                        throw new TraverserException(obj, "Multiple descriptions specified for a single block. Description meta-data was ignored.");
                    });
            }
        }
    }
}