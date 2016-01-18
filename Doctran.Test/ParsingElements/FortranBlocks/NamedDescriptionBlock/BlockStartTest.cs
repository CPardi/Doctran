// <copyright file="BlockStartTest.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Test.ParsingElements.FortranBlocks.NamedDescriptionBlock
{
    using System;
    using System.Collections.Generic;
    using DescriptionBlock;
    using Doctran.Parsing;
    using Doctran.ParsingElements.FortranBlocks;
    using NUnit.Framework;
    using TestUtilities;

    [TestFixture]
    public class BlockStartTest
    {
        private static readonly NamedDescriptionBlock Block = new NamedDescriptionBlock();

        [Test]
        [Category("Unit")]
        [Category("Descriptions")]
        [TestCaseSource(typeof(NamedDescriptionStrings), nameof(NamedDescriptionStrings.TestCases))]
        public void ValidDeclarations(string linesString, Action<IEnumerable<IFortranObject>> makeAssertions)
            => TestingUtils.BlockStartCheck(Block, new IFortranBlock[] { new SourceBlock("FreeFormatFortran") }, linesString);
    }
}