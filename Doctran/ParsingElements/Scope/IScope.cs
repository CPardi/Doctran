// <copyright file="IScope.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.ParsingElements.Scope
{
    using System.Collections.Generic;
    using Helper;
    using Parsing;

    public interface IScope
    {
        IEnumerable<IdentifierObjectPair> EntireScope { get; }

        IErrorListener<TraverserException> ErrorListener { get; set; }

        bool Exists<T>(IIdentifier identifier)
            where T : class, IHasIdentifier;

        bool GetObjectByIdentifier<T>(IIdentifier identifier, out T obj)
            where T : class, IHasIdentifier;

        T GetObjectByIdentifier<T>(IIdentifier identifier)
            where T : class, IHasIdentifier;
    }
}