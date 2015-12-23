// <copyright file="XInformation.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
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

    public class InformationGroup : FortranObject, IInformationGroup
    {
        public InformationGroup(int depth, string name, IEnumerable<IFortranObject> subObjects, List<FileLine> lines)
            : base(subObjects, lines)
        {
            this.Name = name;
            this.Depth = depth;
        }

        public int Depth { get; }

        public string Name { get; }  
    }

    public class InformationValue : FortranObject, IInformationValue
    {
        public InformationValue(int depth, string name, string value, List<FileLine> lines)
            : base(lines) 
        {
            this.Name = name;
            this.Value = value;
            this.Depth = depth;
        }

        public int Depth { get; }

        public string Name { get; }

        public string Value { get; }        
    }
}