// <copyright file="TraverserActions.CheckNames.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.ParsingElements.Traversal
{
    using Parsing;
    using Utilitys;

    public static partial class TraverserActions
    {
        public static ITraverserAction CheckNames
        {
            get
            {
                return new TraverserAction<IHasName>(
                    obj =>
                    {
                        if (obj.Name.IsNullOrEmpty())
                        {
                            throw new TraverserException(obj, "Object has an empty name.");
                        }
                    });
            }
        }
    }
}