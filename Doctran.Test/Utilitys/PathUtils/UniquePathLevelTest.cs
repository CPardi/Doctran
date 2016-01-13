using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doctran.Test.Utilitys.PathUtils
{
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
