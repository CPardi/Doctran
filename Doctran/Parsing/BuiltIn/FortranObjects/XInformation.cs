// <copyright file="XInformation.cs" company="Christopher Pardi">
//     Copyright � 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing.BuiltIn.FortranObjects
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Comments;
    using Helper;

    public class XInformation : XFortranObject, IInformation
    {
        public XInformation(int depth, string name, string value, List<FileLine> lines)
            : base(name, lines) 
        {
            this.Name = name;
            this.Value = value;
            this.Depth = depth;
        }

        public XInformation(int depth, string name, string value, IEnumerable<IFortranObject> subObjects, List<FileLine> lines)
            : base(name, subObjects, lines)
        {
            this.Name = name;
            this.Value = value;
            this.Depth = depth;
        }

        public int Depth { get; }

        public string Name { get; }

        public string Value { get; }

        public override XElement XEle()
        {
            return string.IsNullOrEmpty(this.Value)
                ? new XElement(this.XElementName, this.SubObjects.Select(sinfo => (sinfo as XInformation).XEle()))
                : XElement.Parse("<" + this.XElementName + ">" + this.Value + "</" + this.XElementName + ">");
        }
    }
}