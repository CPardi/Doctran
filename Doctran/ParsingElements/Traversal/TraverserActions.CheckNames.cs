// <copyright file="TraverserActions.CheckNames.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.ParsingElements.Traversal
{
    using System;
    using Parsing;
    using Utilitys;

    public static partial class TraverserActions
    {
        public static ITraverserAction CheckNotEmpty<T>(string propertyName, Func<T, string> getValue)
            where T : IFortranObject
        {
            return new TraverserAction<T>(
                (obj, errLis) =>
                {
                    if (getValue(obj).IsNullOrEmpty())
                    {
                        errLis.Error(new TraverserException(obj, $"A '{Names.OfType(obj.GetType()).ToLower()}' has an empty '{propertyName}'."));
                    }
                });
        }

    }
}