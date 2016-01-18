// <copyright file="UniquePathLevelTest.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Test.Utilitys.PathUtils
{
    using System;
    using Doctran.Utilitys;
    using NUnit.Framework;

    [TestFixture]
    public class UniquePathLevelTest
    {
        [Test]
        [TestCase(0, "File1", "File2", "File3")]
        [TestCase(1, "Dir1/File1", "Dir2/File1")]
        [TestCase(2, "Dir1/SubDir1/File1", "Dir2/SubDir1/File1")]
        [TestCase(0, "Dir1/SubDir1/File1", "Dir1/SubDir1/File2")]
        [TestCase(0, "Dir1/SubDir1/File1", "Dir1/SubDir1/File1", ExpectedException = typeof(InvalidOperationException))]
        public void Test(int expected, params string[] paths)
        {
            Assert.AreEqual(expected, PathUtils.UniquePathLevel(paths));
        }
    }
}