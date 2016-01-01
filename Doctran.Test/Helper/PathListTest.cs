// <copyright file="PathListTest.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Test.Helper
{
    using System;
    using System.IO;
    using System.Linq;
    using Doctran.Helper;
    using NUnit.Framework;

    [TestFixture]
    [Category("Unit")]
    public class PathCollectionTest
    {
        private readonly string _testDir = Path.GetFullPath(@"..\..\TestFiles\PathCollection\");

        private string _originalDirectory;

        [Test(Description = "Add a path with a double wildcard and extension.")]
        public void DirectoryDoubleWildcardWithExtensionMatch()
        {
            var pathColl = new PathList();
            pathColl.Add(@"Folder1\**\*.txt");
            Assert.AreEqual(4, pathColl.Count);
        }

        [Test(Description = "Add a path with a double wildcard and extension.")]
        public void DirectoryWildcardWithExtensionMatch()
        {
            var pathColl = new PathList();
            pathColl.Add(@"Folder1\*.txt");
            Assert.AreEqual(1, pathColl.Count);
        }

        [Test(Description = "Search for a double wildcard with a non existant directory.")]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void DoubleWildcardNoDirectory()
        {
            var pathColl = new PathList { @"NoDirectory\**\*.txt" };
            Assert.AreEqual(4, pathColl.Count);
        }

        [Test(Description = "Add a path with no wildcards to a path that exists.")]
        public void FileExistsAtNormalPath()
        {
            var pathColl = new PathList { "File1.txt" };
            Assert.AreEqual(1, pathColl.Count);
            Assert.AreEqual(Path.GetFullPath("File1.txt"), pathColl.First());
        }

        [Test(Description = "Add a path with a wildcard.")]
        public void FileNameWildcard()
        {
            var pathColl = new PathList();
            pathColl.Add("*");
            Assert.AreEqual(2, pathColl.Count);
        }

        [Test(Description = "Add a path with no wildcards to a path that does not exist.")]
        [ExpectedException(typeof(FileNotFoundException))]
        public void FileNotFoundAtNormalPath()
        {
            var pathColl = new PathList();
            pathColl.Add("NotExistingFile");
        }

        [Test(Description = "Try to remove an item using a path with wildcards. This should be invalid.")]
        [ExpectedException(typeof(ArgumentException))]
        public void RemoveWildCardPath()
        {
            var pathColl = new PathList();
            pathColl.Add(@"Folder1\**\*.txt");
            pathColl.Remove(@"Folder1\**\*.txt");
        }

        [SetUp]
        public void SetUp()
        {
            _originalDirectory = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(_testDir);
        }

        [TearDown]
        public void TearDown()
        {
            Directory.SetCurrentDirectory(_originalDirectory);
        }

        [Test(Description = "Search for a wildcard with a non existant directory.")]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void WildcardNoDirectory()
        {
            var pathColl = new PathList();
            pathColl.Add(@"NoDirectory\*.txt");
            Assert.AreEqual(4, pathColl.Count);
        }

        [Test(Description = "Add a path with wildcard and extension, with file matches.")]
        public void WildcardWithExtensionMatch()
        {
            var pathColl = new PathList();
            pathColl.Add("*.txt");
            Assert.AreEqual(2, pathColl.Count);
        }

        [Test(Description = "Add a path with wildcard and extension, without file matches.")]
        public void WildcardWithExtensionNoMatch()
        {
            var pathColl = new PathList();
            pathColl.Add("*.noMatch");
            Assert.AreEqual(0, pathColl.Count);
        }
    }
}