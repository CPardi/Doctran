// <copyright file="InformationValue.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.ParsingElements.FortranObjects
{
    using System.Collections.Generic;
    using Helper;
    using Parsing;

    public class InformationValue : IInformationValue, IHasLines
    {
        public InformationValue(int depth, string name, string value, List<FileLine> lines)
        {
            this.Name = name;
            this.Value = value;
            this.Depth = depth;
            this.Lines = lines;
        }

        public int Depth { get; }

        public List<FileLine> Lines { get; }

        public string Name { get; }

        public string Value { get; }

        public IContainer Parent { get; set; }
    }
}