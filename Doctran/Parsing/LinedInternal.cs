// <copyright file="LinedInternal.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing
{
    using System.Collections.Generic;
    using Helper;
    using ParsingElements;

    public abstract class LinedInternal : Container, IContained, IHasLines
    {
        protected LinedInternal(IEnumerable<IContained> subObjects, List<FileLine> lines)
            : base(subObjects)
        {
            this.Lines = lines;
        }

        public List<FileLine> Lines { get; }

        public IContainer Parent { get; set; }
    }
}