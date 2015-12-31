// <copyright file="TraverserActions.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.ParsingElements.Traversal
{
    using System.Linq;
    using Parsing;

    public static partial class TraverserActions
    {
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