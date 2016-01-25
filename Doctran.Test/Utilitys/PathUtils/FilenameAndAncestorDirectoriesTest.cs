// <copyright file="FilenameAndAncestorDirectoriesTest.cs" company="Christopher Pardi">
// <copyright file="FilenameAndAncestorDirectoriesTest.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Test.Utilitys.PathUtils
{
    using Doctran.Utilitys;
    using NUnit.Framework;

    [TestFixture]
    public class FilenameAndAncestorDirectoriesTest
    {
        [Test]
        [TestCase("File", "Dir1/File", 0)]
        [TestCase("Dir1/File", "Dir1/File", 1)]
        [TestCase("Dir1/File", "Dir2/Dir1/File", 1)]
        [TestCase("Dir2/Dir1/File", "Dir2/Dir1/File", 2)]
        [TestCase("Dir2/Dir1/File", "Dir2/Dir1/File", 3)]
        public void Test(string expected, string path, int numDirectories)
        {
            Assert.AreEqual(expected, PathUtils.FilenameAndAncestorDirectories(path, numDirectories));
        }
    }
}
