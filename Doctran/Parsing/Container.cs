// <copyright file="Container.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing
{
    using System.Collections.Generic;
    using ParsingElements;
    using Utilitys;

    public abstract class Container : IContainer
    {
        protected Container(IEnumerable<IContained> subObjects)
        {
            this.AddSubObjects(subObjects);
        }

        public string ObjectName => Names.OfType(this.GetType());

        public List<IContained> SubObjects { get; } = new List<IContained>();

        public void AddSubObject(IContained containedItem) => CollectionUtils.AddSubObject(this, containedItem);

        public void AddSubObjects(IEnumerable<IContained> containedItems) => CollectionUtils.AddSubObjects(this, containedItems);

        public void RemoveSubObject(IContained containedItem) => CollectionUtils.RemoveSubObject(this, containedItem);

        public void RemoveSubObjects(IEnumerable<IContained> containedItems) => CollectionUtils.RemoveSubObjects(this, containedItems);
    }
}