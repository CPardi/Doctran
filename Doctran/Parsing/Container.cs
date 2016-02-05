// <copyright file="Container.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using ParsingElements;
    using Utilitys;

    public abstract class Container : IContainer
    {
        private readonly List<IContained> _subObjects = new List<IContained>();

        protected Container(IEnumerable<IContained> subObjects)
        {
            this.AddSubObjects(subObjects);
        }

        public string ObjectName => Names.OfType(this.GetType());

        public ReadOnlyCollection<IContained> SubObjects => _subObjects.AsReadOnly();

        public void AddSubObject(IContained containedItem) => CollectionUtils.AddSubObject(_subObjects, this, containedItem);

        public void AddSubObjects(IEnumerable<IContained> containedItems) => CollectionUtils.AddSubObjects(_subObjects, this, containedItems);

        public void InsertSubObject(int index, IContained containedItem) => CollectionUtils.InsertSubObject(_subObjects, this, index, containedItem);

        public void RemoveSubObject(IContained containedItem) => CollectionUtils.RemoveSubObject(_subObjects, this, containedItem);

        public void RemoveSubObjects(IEnumerable<IContained> containedItems) => CollectionUtils.RemoveSubObjects(this, containedItems);
    }
}