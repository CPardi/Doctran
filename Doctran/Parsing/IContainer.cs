// <copyright file="IContainer.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing
{
    using System.Collections.Generic;

    public interface IContainer : IFortranObject
    {
        List<IContained> SubObjects { get; }

        void AddSubObject(IContained containedItem);

        void AddSubObjects(IEnumerable<IContained> containedItems);

        void RemoveSubObject(IContained containedItem);

        void RemoveSubObjects(IEnumerable<IContained> containedItems);
    }
}