// <copyright file="Source.cs" company="Christopher Pardi">
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

    public class Source : LinedInternal, ISource
    {
        public Source(string language, IEnumerable<IContained> subObjects, List<FileLine> lines)
            : base(subObjects, lines)
        {
            this.Language = language;
        }

        public IIdentifier Identifier => new Identifier($"{this.Language} source");

        public string Language { get; }
    }
}