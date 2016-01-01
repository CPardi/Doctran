// <copyright file="AbstractList.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Helper
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     Provides default implementations of the properties and methods inherited from <see cref="IList{T}" /> and
    ///     <see cref="IList" />. The type then allows a neat way to create a custom List class, where only modified properties
    ///     and methods are required to be overriden.
    /// </summary>
    /// <typeparam name="T">The generic type of the elements within the list.</typeparam>
    public abstract class AbstractList<T> : IList<T>, IList
    {
        public virtual int Count => this.InternalList.Count;

        public virtual bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public virtual bool IsReadOnly => this.InternalList.IsReadOnly;

        public virtual bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public virtual object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        protected virtual IList<T> InternalList { get; set; } = new List<T>();

        public virtual T this[int index]
        {
            get { return this.InternalList[index]; }
            set { this.InternalList[index] = value; }
        }

        object IList.this[int index]
        {
            get { return this[index]; }
            set { this[index] = (T)value; }
        }

        public virtual int Add(object item)
        {
            try
            {
                this.Add((T)item);
            }
            catch (InvalidCastException)
            {
                return -1;
            }

            return this.Count - 1;
        }

        public virtual void Add(T item) => this.InternalList.Add(item);

        public virtual void Clear() => this.InternalList.Clear();

        public virtual bool Contains(object value)
        {
            return this.Contains((T)value);
        }

        public virtual bool Contains(T itemList) => this.InternalList.Contains(itemList);

        public virtual void CopyTo(Array array, int index)
        {
            this.CopyTo(array.Cast<T>().ToArray(), index);
        }

        public virtual void CopyTo(T[] array, int arrayIndex) => this.InternalList.CopyTo(array, arrayIndex);

        public virtual IEnumerator<T> GetEnumerator() => this.InternalList.GetEnumerator();

        public virtual int IndexOf(object value)
        {
            return this.IndexOf((T)value);
        }

        public virtual int IndexOf(T item) => this.InternalList.IndexOf(item);

        public virtual void Insert(int index, object value)
        {
            this.Insert(index, (T)value);
        }

        public virtual void Insert(int index, T item) => this.InternalList.Insert(index, item);

        public virtual void Remove(object value)
        {
            this.Remove((T)value);
        }

        public virtual bool Remove(T item) => this.InternalList.Remove(item);

        public virtual void RemoveAt(int index) => this.InternalList.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)this.InternalList).GetEnumerator();
    }
}