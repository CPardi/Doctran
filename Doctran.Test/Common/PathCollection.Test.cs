using NUnit.Framework;
using System.Reflection;
using System.IO;
using System;
using System.Linq;

namespace Doctran.Test.Common
{
    using Helper;

    [TestFixture]
    public class PathCollectionTest
    {        
        private string _originalDirectory;
        private string _test_dir = @"C:\Documents\Programming\VisualStudio\Projects\Doctran\Doctran.Test\TestFiles\PathCollection\";

        [SetUp]
        public void SetUp()
        {
            _originalDirectory = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(_test_dir);
        }

        [TearDown]
        public void TearDown()
        {
            Directory.SetCurrentDirectory(_originalDirectory);
        }

        [Test(Description = "Add a path with no wildcards to a path that exists.")]
        [Category("Precise")]
        public void FileExistsAtNormalPath()
        {
            PathList pathColl = new PathList();
            pathColl.Add("File1.txt");
            Assert.AreEqual(pathColl.Count, 1);
            Assert.AreEqual(pathColl.First(), "File1.txt");
        }

        [Test(Description = "Add a path with no wildcards to a path that does not exist.")]
        [Category("Precise")]
        [ExpectedException(typeof(FileNotFoundException))]
        public void FileNotFoundAtNormalPath()
        {
            PathList pathColl = new PathList();
            pathColl.Add("NotExistingFile");
        }

        [Test(Description = "Add a path with a wildcard.")]
        [Category("Precise")]
        public void FileNameWildcard()
        {
            PathList pathColl = new PathList();
            pathColl.Add("*");
            Assert.AreEqual(pathColl.Count, 2);
        }

        [Test(Description = "Add a path with wildcard and extension, with file matches.")]
        [Category("Precise")]
        public void WildcardWithExtensionMatch()
        {
            PathList pathColl = new PathList();
            pathColl.Add("*.txt");
            Assert.AreEqual(pathColl.Count, 2);
        }

        [Test(Description = "Add a path with wildcard and extension, without file matches.")]
        [Category("Precise")]
        public void WildcardWithExtensionNoMatch()
        {
            PathList pathColl = new PathList();
            pathColl.Add("*.noMatch");
            Assert.AreEqual(pathColl.Count, 0);
        }

        [Test(Description = "Add a path with a double wildcard and extension.")]
        [Category("Precise")]
        public void DirectoryWildcardWithExtensionMatch()
        {
            PathList pathColl = new PathList();
            pathColl.Add(@"Folder1\*.txt");
            Assert.AreEqual(pathColl.Count, 1);
        }

        [Test(Description = "Add a path with a double wildcard and extension.")]
        [Category("Precise")]
        public void DirectoryDoubleWildcardWithExtensionMatch()
        {
            PathList pathColl = new PathList();
            pathColl.Add(@"Folder1\**\*.txt");
            Assert.AreEqual(pathColl.Count, 4);
        }

        [Test(Description = "Search for a double wildcard with a non existant directory.")]
        [Category("Precise")]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void DoubleWildcardNoDirectory()
        {
            PathList pathColl = new PathList();
            pathColl.Add(@"NoDirectory\**\*.txt");
            Assert.AreEqual(pathColl.Count, 4);
        }

        [Test(Description = "Search for a wildcard with a non existant directory.")]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        [Category("Precise")]
        public void WildcardNoDirectory()
        {
            PathList pathColl = new PathList();
            pathColl.Add(@"NoDirectory\*.txt");
            Assert.AreEqual(pathColl.Count, 4);
        }

        [Test(Description = "Try to remove an item using a path with wildcards. This should be invalid.")]
        [ExpectedException(typeof(ArgumentException))]
        [Category("Precise")]
        public void RemoveWildCardPath()
        {
            PathList pathColl = new PathList();
            pathColl.Add(@"Folder1\**\*.txt");
            pathColl.Remove(@"Folder1\**\*.txt");
        }
    }
}
