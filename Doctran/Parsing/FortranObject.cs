// <copyright file="FortranObject.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing
{
    using System.Collections.Generic;
    using System.Linq;
    using Helper;

    public abstract class FortranObject : IFortranObject, IContainer, IContained
    {
        protected FortranObject()
        {
            this.SubObjects = new List<IContained>();
            this.Lines = new List<FileLine>();
        }

        protected FortranObject(List<FileLine> lines)
            : this()
        {
            this.Lines = lines;
        }

        protected FortranObject(IEnumerable<IContained> subObjects, List<FileLine> lines)
            : this(lines)
        {
            this.AddSubObjects(subObjects);
        }

        public List<FileLine> Lines { get; }

        public IContainer Parent { get; set; }

        public List<IContained> SubObjects { get; }

        public void AddSubObject(IContained obj)
        {
            obj.Parent = this;
            this.SubObjects.Add(obj);
        }

        public void AddSubObjects(IEnumerable<IContained> objs)
        {
            var objList = objs as IList<IContained> ?? objs.ToList();
            foreach (var o in objList)
            {
                o.Parent = this;
            }

            this.SubObjects.AddRange(objList);
        }

        public T GoUpTillType<T>()
            where T : class, IContainer
        {
            IContained obj = this;
            while (obj != null && !(obj is T))
            {
                obj = obj.Parent as IContained;
            }

            return obj as T;
        }

        public void RemoveSubObject(IContained obj)
        {
            this.SubObjects.Remove(obj);
        }

        public void RemoveSubObjects(IEnumerable<IContained> objs)
        {
            foreach (var obj in objs)
            {
                this.RemoveSubObject(obj);
            }
        }
    }
}