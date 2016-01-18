// <copyright file="BlockStartTest.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Test.ParsingElements.FortranBlocks.DescriptionBlock
{
    using System;
    using System.Collections.Generic;
    using Doctran.Parsing;
    using Doctran.ParsingElements.FortranBlocks;
    using NUnit.Framework;
    using TestUtilities;

    [TestFixture]
    public class BlockStartTest
    {
        private static readonly DescriptionBlock Block = new DescriptionBlock();

        [Test]
        [Category("Unit")]
        [Category("Descriptions")]
        [TestCaseSource(typeof(DescriptionStrings), nameof(DescriptionStrings.TestCases))]
        public void ValidDeclarations(string linesString, Action<IEnumerable<IFortranObject>> makeAssertions)
            => TestingUtils.BlockStartCheck(Block, new IFortranBlock[] { new SourceBlock("FreeFormatFortran") }, linesString);
    }
}