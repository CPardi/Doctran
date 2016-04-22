// <copyright file="InformationGroup.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.ParsingElements.FortranObjects
{
    using System.Collections.Generic;
    using Helper;
    using Information;
    using Parsing;

    public class InformationGroup : LinedInternal, IInformationGroup
    {
        public InformationGroup(int depth, string name, IEnumerable<IContained> subObjects, List<FileLine> lines)
            : base(subObjects, lines)
        {
            this.Name = name;
            this.Depth = depth;
        }

        public int Depth { get; }

        public string Name { get; }
    }
}