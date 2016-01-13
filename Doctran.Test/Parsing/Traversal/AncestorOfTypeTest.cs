namespace Doctran.Test.Parsing.Traversal
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using Doctran.Helper;
    using Doctran.Parsing;
    using Doctran.ParsingElements.FortranObjects;
    using NUnit.Framework;

    [TestFixture]
    public class AncestorOfTypeTest
    {
        [Test]
        public void OneDeepRootAncestor()
        {
            var file1 = new SourceFile(string.Empty, @"C:\", new IContained[] { }, string.Empty, new List<FileLine>());
            var project = new Project(new[] { file1 });

            Assert.AreEqual(project, Traversal.AncestorOfType<Project>(file1));
        }

        [Test]
        public void TwoDeepRootAncestor()
        {
            var desc = new Description(new XElement("Basic"), new XElement("Detail"), new List<FileLine>());
            var file1 = new SourceFile(string.Empty, @"C:\", new[] { desc }, string.Empty, new List<FileLine>());
            var project = new Project(new[] { file1 });

            Assert.AreEqual(project, Traversal.AncestorOfType<Project>(desc));
        }

        [Test]
        public void OneDeepChildAncestor()
        {
            var desc = new Description(new XElement("Basic"), new XElement("Detail"), new List<FileLine>());
            var file1 = new SourceFile(string.Empty, @"C:\", new[] { desc }, string.Empty, new List<FileLine>());
            var project = new Project(new[] { file1 });

            Assert.AreEqual(file1, Traversal.AncestorOfType<SourceFile>(desc));
        }

        [Test]
        public void NoAncestor()
        {
            var desc = new Description(new XElement("Basic"), new XElement("Detail"), new List<FileLine>());
            var file1 = new SourceFile(string.Empty, @"C:\", new[] { desc }, string.Empty, new List<FileLine>());
            var project = new Project(new[] { file1 });

            Assert.AreEqual(null, Traversal.AncestorOfType<InformationValue>(desc));
        }
    }
}
