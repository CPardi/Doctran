//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Doctran.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Helper;

    public abstract class FortranObject
	{
        public FortranObject parent { get; private set; }
		public string Name { get; set; }
        public List<FortranObject> SubObjects { get; private set; }
		public List<FileLine> lines { get; set; }

		protected FortranObject() 
        {
            this.SubObjects = new List<FortranObject>();
            this.lines = new List<FileLine>();
        }

        protected FortranObject(List<FileLine> lines)
            : this()
        {
            this.lines = lines;
        }

        protected FortranObject(string name, List<FileLine> lines)
            : this(lines)
        {
            this.Name = name;
        }

        protected FortranObject(IEnumerable<FortranObject> sub_objects, List<FileLine> lines)
            : this(lines)
        {
            this.AddSubObjects(sub_objects);
        }

        protected FortranObject(string name, IEnumerable<FortranObject> sub_objects, List<FileLine> lines)
            : this(sub_objects, lines)
        {
            this.Name = name;
        }

        public void AddSubObject(FortranObject obj)
        {
            obj.parent = this;
            this.SubObjects.Add(obj);
        }

        public void AddSubObjects(IEnumerable<FortranObject> objs)
        {
            foreach (var o in objs)
                o.parent = this;
            this.SubObjects.AddRange(objs);
        }

        public string Identifier
		{
			get
			{
				return this.GetIdentifier().ToLower();
			}
		}

		protected abstract string GetIdentifier();
	
		public T GoUpTillType<T>() where T : FortranObject
		{
			FortranObject obj = this;
			while (!(obj is T))
				obj = obj.parent;
			return obj as T;
		}

        public void OrderSubObjectsBy(Func<FortranObject,int> keySelector)
        {
            this.SubObjects = this.SubObjects.OrderBy(keySelector).ToList();
        }

		public List<R> SubObjectsOfType<R>() 
			where R : FortranObject
		{
			IEnumerable<R> a = from obj in this.SubObjects
				   where obj is R
				   select (R)obj;
			return a.ToList<R>();
		}

		public List<FortranObject> SubObjectsNotOfType<R>()
			where R : FortranObject
		{
			IEnumerable<FortranObject> a = from obj in this.SubObjects
							   where !(obj is R)
							   select obj;
			return a.ToList();
		}
	}
}
