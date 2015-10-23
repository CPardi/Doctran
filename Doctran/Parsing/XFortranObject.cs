//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Doctran.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using FortranObjects;
    using Helper;
    using Output;
    using Reporting;
    using Utilitys;

    public abstract class XFortranObject : FortranObject
	{
		public string XElementName;

		protected XFortranObject() { }

        protected XFortranObject(string xElementName, List<FileLine> lines)
            : base(lines)
        {
            this.XElementName = xElementName;
        }

        protected XFortranObject(string xElementName, IEnumerable<FortranObject> subObjects, List<FileLine> lines)
			: base(subObjects, lines)
		{
			this.XElementName = xElementName;
		}

        protected XFortranObject(string name, IEnumerable<FortranObject> subObjects, string xElementName, List<FileLine> lines)
			: base(name, subObjects, lines)
		{
			this.XElementName = xElementName;
		}

		private List<XElement> NonFortranObject(FortranObject obj)
		{
			var xeles = new List<XElement>();
			xeles.AddRange(GroupXEle(obj));
			xeles.AddRange(
				obj.SubObjectsNotOfType<XFortranObject>().SelectMany(NonFortranObject)
				);
			return xeles;
		}

        private IEnumerable<ObjectGroup> ObjectGroups => this.GoUpTillType<File>().ObjectGroups;

        private List<XElement> GroupXEle(FortranObject obj)
		{
			var xeles = new List<XElement>();
			foreach (var grp in this.ObjectGroups)
			{
				List<XElement> xele =
					(from sObj in obj.SubObjectsOfType<XFortranObject>()
					 where grp.Is(sObj)
					 let grpXele = sObj.XEle()
					 where grpXele != null
					 select grpXele).ToList();

			    if (!xele.Any())
			    {
			        continue;
			    }

			    try
			    {
			        xeles.Add(grp.XEle(xele));
			    }
			    catch (InvalidOperationException)
			    {
			        xeles.Add(grp.XEle(xele.First()));
			        Report.Warning((pub) =>
			        {
			            pub.AddWarningDescription("Error in XML parsing.");
			            pub.AddReason($"Expected a single '{obj.GetType().Name}' block, but multiple were found. Only the first occurence will be used.");
			            pub.AddLocation($"{obj.GetType().Name} '{obj.Name}' within '{this.GoUpTillType<File>().Name}'.");
			        });
			    }
			}
			return xeles;
		}

		public virtual XElement XEle()
		{
			XElement xele = new XElement(this.XElementName);
			xele.Add(new XElement("Name", this.Name));
			xele.Add(new XElement("Identifier", this.Identifier));

			if (this.lines.Count != 0)
				xele.Add(new XElement("Lines",
							new XElement("First", Math.Max(1, this.lines.First().Number)),
							new XElement("Last", this.lines.Last().Number)
							));

			xele.Add(GroupXEle(this));
			var subXEles = this.SubObjectsNotOfType<XFortranObject>().SelectMany
				(NonFortranObject);
			if (subXEles.Any())
				xele.Add(subXEles);
			return xele;
		}
	}
}