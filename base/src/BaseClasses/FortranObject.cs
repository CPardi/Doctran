//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

using Doctran.Fbase.Common;
using Doctran.BaseClasses;

using Doctran.Fbase.Projects;

namespace Doctran.BaseClasses
{
	public abstract class FortranObject
	{
        public FortranObject parent { get; private set; }
		public String Name { get; set; }
        public List<FortranObject> SubObjects { get; private set; }
		public List<FileLine> lines = new List<FileLine>();

		protected FortranObject() 
        {
            this.SubObjects = new List<FortranObject>();
        }

        protected FortranObject(List<FileLine> lines)
            : this()
        {
            this.lines = lines;
        }

        protected FortranObject(String name, List<FileLine> lines)
            : this(lines)
        {
            this.Name = name;
        }

        protected FortranObject(IEnumerable<FortranObject> sub_objects, List<FileLine> lines)
            : this(lines)
        {
            this.AddSubObjects(sub_objects);
        }

        protected FortranObject(String name, IEnumerable<FortranObject> sub_objects, List<FileLine> lines)
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

        public String Identifier
		{
			get
			{
				return this.GetIdentifier().ToLower();
			}
		}

		protected abstract String GetIdentifier();
	
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

	public class UnclosedBlockException : ApplicationException
	{
		public FortranBlock block;

		public UnclosedBlockException(FortranBlock block)
			: base()
		{
			this.block = block;
		}

		public override string ToString()
		{
			return "A " + this.block.GetType().Name + " has not been closed";
		}
	}
}
