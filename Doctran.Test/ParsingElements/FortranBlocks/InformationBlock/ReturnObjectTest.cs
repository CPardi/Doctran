// <copyright file="ReturnObjectTest.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Test.ParsingElements.FortranBlocks.InformationBlock
{
    using System;
    using System.Collections.Generic;
    using Doctran.Parsing;
    using Doctran.ParsingElements.FortranBlocks;
    using NUnit.Framework;
    using TestUtilities;

    [TestFixture]
    public class ReturnObjectTest
    {
        private static readonly InformationBlock Block = new InformationBlock(1);

        [Test]
        [Category("Unit")]
        [Category("Descriptions")]
        [TestCaseSource(typeof(InformationStrings), nameof(InformationStrings.TestCases))]
        public void ValidDeclarations(string linesString, Action<IEnumerable<IFortranObject>> makeAssertions)
            => TestingUtils.ReturnObjectCheck(Block, linesString, makeAssertions);
    }
}