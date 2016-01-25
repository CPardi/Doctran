// <copyright file="FilenameAndAncestorDirectoriesTest.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Test.Utilitys.PathUtils
{
    using System.Collections;
    using Doctran.Utilitys;
    using NUnit.Framework;

    [TestFixture]
    public class SplitIntoDirectoriesTest
    {
        [Test]
        [TestCaseSource(typeof(SplitIntoDirectoriesTest), nameof(SplitIntoDirectoriesTest.Data))]
        public void Test(string path, string[] expected)
        {
            var actual = PathUtils.SplitIntoDirectories(path);

            Assert.AreEqual(expected.Length, actual.Length);

            for(var i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        public static IEnumerable Data
        {
            get
            {
                yield return new TestCaseData("Dir1/File", new [] { "Dir1", "File" });
                yield return new TestCaseData("Dir2/Dir1/File", new [] { "Dir2", "Dir1", "File" });
            }
        }
    }
}
