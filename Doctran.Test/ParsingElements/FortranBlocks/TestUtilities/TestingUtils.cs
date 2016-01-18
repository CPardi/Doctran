// <copyright file="TestingUtils.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Test.ParsingElements.FortranBlocks.TestUtilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Doctran.Parsing;
    using NUnit.Framework;

    public static class TestingUtils
    {
        public static void BlockEndCheck(IFortranBlock block, IEnumerable<IFortranBlock> ancestors, string linesString)
        {
            var lines = StringUtils.ConvertToFileLineList(linesString);
            Assert.IsTrue(block.BlockEnd(ancestors, lines, lines.Count - 1));
        }

        public static void BlockStartCheck(IFortranBlock block, IEnumerable<IFortranBlock> ancestors, string linesString)
        {
            var lines = StringUtils.ConvertToFileLineList(linesString);
            Assert.IsTrue(block.BlockStart(ancestors, lines, 0));
        }

        public static void ReturnObjectCheck(IFortranBlock block, string linesString, Action<IEnumerable<IFortranObject>> makeAssertions)
        {
            ReturnObjectCheck(block, linesString, new List<IContained>(), makeAssertions);
        }

        public static void ReturnObjectCheck(IFortranBlock block, string linesString, IEnumerable<IContained> subObjects, Action<IEnumerable<IFortranObject>> makeAssertions)
        {
            var lines = StringUtils.ConvertToFileLineList(linesString);
            var actual = block.ReturnObject(subObjects, lines)
                .ToList<IFortranObject>();
            makeAssertions(actual);
        }
    }
}