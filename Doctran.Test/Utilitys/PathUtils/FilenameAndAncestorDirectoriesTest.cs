using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
