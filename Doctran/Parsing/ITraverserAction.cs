// <copyright file="ITraverserAction.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing
{
    using System;

    public interface ITraverserAction
    {
        Action<object> Act { get; }

        Type ForType { get; }
    }

    public interface ITraverserAction<in T> : ITraverserAction
    {
        new Action<T> Act { get; }
    }
}