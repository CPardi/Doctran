// <copyright file="DescriptionBlock.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing.BuiltIn.FortranBlocks
{
    using System.Collections.Generic;
    using FortranObjects;
    using Helper;

    public class SourceBlock : FortranBlock
    {
        private readonly string _language;

        public SourceBlock(string language)
            : base("Source", true, false)
        {
            _language = language;
        }

        public override bool BlockStart(string parentBlockName, List<FileLine> lines, int lineIndex)
        {
            return lineIndex == 0;
        }

        public override bool BlockEnd(string parentBlockName, List<FileLine> lines, int lineIndex)
        {
            return lineIndex + 1 >= lines.Count;
        }

        public override IEnumerable<FortranObject> ReturnObject(IEnumerable<IFortranObject> subObjects, List<FileLine> lines)
        {
            yield return new Source(_language ,subObjects, lines);
        }
    }
}