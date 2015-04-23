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
	public abstract class XFortranObject : FortranObject
    {
        public String XElement_Name;

        protected XFortranObject() { }
        
        protected XFortranObject(FortranObject parent, String XElement_Name, List<FileLine> lines, bool ContainsBlocks)
            : base(parent, lines, ContainsBlocks)
        {
            this.XElement_Name = XElement_Name;
        }

        protected XFortranObject(FortranObject parent, String name, String XElement_Name, List<FileLine> lines, bool ContainsBlocks)
            : base(parent, name, lines, ContainsBlocks)
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
					 let grp_xele = sObj.XEle()
					 where grp_xele != null
					 select grp_xele).ToList();

				if (xele.Any())
                    xeles.Add(grp.XEle(xele));
            }
            return xeles;
        }

        public virtual XElement XEle()
        {
            XElement xele = new XElement(this.XElement_Name);
            xele.Add(new XElement("Name", this.Name));
            xele.Add(new XElement("Identifier", this.Identifier));

            xele.Add(GroupXEle(this));
            var subXEles = this.SubObjectsNotOfType<XFortranObject>().SelectMany
                (sObj => NonFortranObject(sObj));
            if (subXEles.Any())
                xele.Add(subXEles);
            return xele;
        }
    }
}