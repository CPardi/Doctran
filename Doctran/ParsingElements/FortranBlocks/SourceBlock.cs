// <copyright file="SourceBlock.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.ParsingElements.FortranBlocks
{
    using System.Collections.Generic;
    using FortranObjects;
    using Helper;
    using Parsing;

    public class SourceBlock : IFortranBlock
    {
        private readonly string _language;

        public SourceBlock(string language)
        {
            _language = language;
        }

        public bool CheckInternal => true;

        public bool ExplicitEnd => false;

        public string Name => "Source";

        public bool BlockEnd(IEnumerable<IFortranBlock> ancestors, List<FileLine> lines, int lineIndex) => lineIndex + 1 >= lines.Count;

        public bool BlockStart(IEnumerable<IFortranBlock> ancestors, List<FileLine> lines, int lineIndex) => lineIndex == 0;

        public IEnumerable<IContained> ReturnObject(IEnumerable<IContained> subObjects, List<FileLine> lines)
        {
            yield return new Source(_language, subObjects, lines);
        }
    }
}