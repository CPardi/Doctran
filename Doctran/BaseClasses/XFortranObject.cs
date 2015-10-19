//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using Doctran.Fbase.Common;

namespace Doctran.BaseClasses
{
    using Reporting;

    public abstract class XFortranObject : FortranObject
	{
		public string XElement_Name;

		protected XFortranObject() { }

        protected XFortranObject(string XElement_Name, List<FileLine> lines)
            : base(lines)
        {
            this.XElement_Name = XElement_Name;
        }

        protected XFortranObject(string XElement_Name, IEnumerable<FortranObject> sub_objects, List<FileLine> lines)
			: base(sub_objects, lines)
		{
			this.XElement_Name = XElement_Name;
		}

        protected XFortranObject(string name, IEnumerable<FortranObject> sub_objects, string XElement_Name, List<FileLine> lines)
			: base(name, sub_objects, lines)
		{
			this.XElement_Name = XElement_Name;
		}

		private List<XElement> NonFortranObject(FortranObject obj)
		{
			List<XElement> xeles = new List<XElement>();
			xeles.AddRange(GroupXEle(obj));
			xeles.AddRange(
				obj.SubObjectsNotOfType<XFortranObject>().SelectMany(sObj => NonFortranObject(sObj))
				);
			return xeles;
		}

		private List<XElement> GroupXEle(FortranObject obj)
		{
			List<XElement> xeles = new List<XElement>();
			foreach (var grp in PluginManager.ObjectGroups)
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
			    catch (InvalidOperationException e)
			    {
			        xeles.Add(grp.XEle(xele.First()));
			        Report.Warning((pub) =>
			        {
			            pub.AddWarningDescription("Error in XML parsing.");
			            pub.AddReason(e.Message);
			            pub.AddLocation(obj.GetType().Name + " " + obj.Name + " within " + this.GoUpTillType<Doctran.Fbase.Files.File>().Name);
			        });
			    }
			}
			return xeles;
		}

		public virtual XElement XEle()
		{
			XElement xele = new XElement(this.XElement_Name);
			xele.Add(new XElement("Name", this.Name));
			xele.Add(new XElement("Identifier", this.Identifier));

			if (this.lines.Count != 0)
				xele.Add(new XElement("Lines",
							new XElement("First", Math.Max(1, this.lines.First().Number)),
							new XElement("Last", this.lines.Last().Number)
							));

			xele.Add(GroupXEle(this));
			var subXEles = this.SubObjectsNotOfType<XFortranObject>().SelectMany
				(sObj => NonFortranObject(sObj));
			if (subXEles.Any())
				xele.Add(subXEles);
			return xele;
		}
	}
}