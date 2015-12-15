// <copyright file="FortranObject.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Helper;

    public abstract class FortranObject : IFortranObject
    {
        protected FortranObject()
        {
            this.SubObjects = new List<IFortranObject>();
            this.Lines = new List<FileLine>();
        }

        protected FortranObject(List<FileLine> lines)
            : this()
        {
            this.Lines = lines;
        }

        protected FortranObject(IEnumerable<IFortranObject> subObjects, List<FileLine> lines)
            : this(lines)
        {
            this.AddSubObjects(subObjects);
        }

        public List<FileLine> Lines { get; }

        public IFortranObject Parent { get; set; }

        public List<IFortranObject> SubObjects { get; private set; }

        public void AddSubObject(IFortranObject obj)
        {
            obj.Parent = this;
            this.SubObjects.Add(obj);
        }

        public void AddSubObjects(IEnumerable<IFortranObject> objs)
        {
            var objList = objs as IList<IFortranObject> ?? objs.ToList();
            foreach (var o in objList)
            {
                o.Parent = this;
            }

            this.SubObjects.AddRange(objList);
        }

        public T GoUpTillType<T>()
            where T : IFortranObject
        {
            IFortranObject obj = this;
            while (!(obj is T))
            {
                obj = obj.Parent;
            }

            return (T)obj;
        }

        public void OrderSubObjectsBy(Func<IFortranObject, int> keySelector)
        {
            this.SubObjects = this.SubObjects.OrderBy(keySelector).ToList();
        }

        public List<IFortranObject> SubObjectsNotOfType<TR>()
            where TR : IFortranObject
        {
            var a = from obj in this.SubObjects
                where !(obj is TR)
                select obj;
            return a.ToList();
        }

        public List<TR> SubObjectsOfType<TR>()
            where TR : IFortranObject
        {
            var a = from obj in this.SubObjects
                where obj is TR
                select (TR)obj;
            return a.ToList();
        }
    }
}