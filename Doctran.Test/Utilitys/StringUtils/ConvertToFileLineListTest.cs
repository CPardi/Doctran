// <copyright file="ConvertToFileLineListTest.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Test.Utilitys.StringUtils
{
    using Doctran.Utilitys;
    using NUnit.Framework;

    [TestFixture]
    public class ConvertToFileLineListTest
    {
        [Test]
        [TestCase("Line 1\nLine 2\nLine 3", new[] { "Line 1", "Line 2", "Line 3" }, Description = "Simple conversion with \n newline character.")]
        [TestCase("Line 1\r\nLine 2\r\nLine 3", new[] { "Line 1", "Line 2", "Line 3" }, Description = "Simple conversion with \r\n newline character.")]
        public static void Check(string source, string[] expected)
        {
            var lines = StringUtils.ConvertToFileLineList(source);

            Assert.AreEqual(expected.Length, lines.Count);
            for (var i = 0; i < lines.Count; i++)
            {
                Assert.AreEqual(expected[i], lines[i].Text);
                Assert.AreEqual(i + 1, lines[i].Number, $"Conversion of '{nameof(source)}' causes incorrect file lines.");
            }
        }
    }
}