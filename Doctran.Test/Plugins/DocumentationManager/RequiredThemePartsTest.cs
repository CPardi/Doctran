namespace Doctran.Test.Plugins.DocumentationManager
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Xml.Linq;
    using Doctran.Parsing;
    using Doctran.Plugins;
    using Doctran.Utilitys;
    using NUnit.Framework;

    [TestFixture]
    public class RequiredThemePartsTest
    {
        public static IEnumerable CommonTestData
        {
            get
            {
                yield return new TestCaseData(new[] { "base" }, new string[] { })
                    .SetName("No source files.");
                yield return new TestCaseData(new[] { "base", "f95", "f03" }, new[] { ".f90" })
                    .SetName("One f90 source file.");
                yield return new TestCaseData(new[] { "base", "f95", "f03" }, new[] { ".f90", ".f90", ".f90", ".f90" })
                    .SetName("Multiple f90 source files.");
            }
        }
        
        public static IEnumerable MultipleTestData
        {
            get
            {
                yield return new TestCaseData(new[] { "base", "f95", "f03" }, new[] { ".f95" })
                    .SetName("One f90 source file.");
                yield return new TestCaseData(new[] { "base", "f95", "f03" }, new[] { ".f90", ".f95", ".f03", ".f90" })
                    .SetName("Multiple f90 source files.");
            }
        }

        [Test]
        [TestCaseSource(nameof(CommonTestData))]
        public void SingleDefinition(string[] parts, string[] extensions)
        {
            DocumentationManager.RegisterDocumentationDefinition("test", ".f90", new TestDocDef());
            var actualParts = DocumentationManager.RequiredThemeParts(extensions).ToArray();
            DocumentationManager.Reset();
            var errorString = $"Expected: {parts.DelimiteredConcat(", ", " and ")}\nFound:    {actualParts.DelimiteredConcat(", ", " and ")}";

            Assert.AreEqual(parts.Length, actualParts.Length, errorString);
            foreach (var actual in actualParts)
            {
                Assert.IsTrue(parts.Contains(actual), errorString);
            }
        }

        [Test]
        [TestCaseSource(nameof(CommonTestData))]
        [TestCaseSource(nameof(MultipleTestData))]
        public void MutlipleDefinition(string[] parts, string[] extensions)
        {
            DocumentationManager.RegisterDocumentationDefinition("test1", ".f90", new TestDocDef());
            DocumentationManager.RegisterDocumentationDefinition("test2", ".f95", new TestDocDef());
            DocumentationManager.RegisterDocumentationDefinition("test3", ".f03", new TestDocDef());

            var actualParts = DocumentationManager.RequiredThemeParts(extensions).ToArray();
            DocumentationManager.Reset();
            var errorString = $"Expected: {parts.DelimiteredConcat(", ", " and ")}\nFound:    {actualParts.DelimiteredConcat(", ", " and ")}";

            Assert.AreEqual(parts.Length, actualParts.Length, errorString);
            foreach (var actual in actualParts)
            {
                Assert.IsTrue(parts.Contains(actual), errorString);
            }
        }

        private class TestDocDef : IDocumentationDefinition
        {
            public string[] ThemePartNames => new[] { "f95", "f03" };

            public XElement HighlightLines(string source)
            {
                throw new NotImplementedException();
            }

            public XElement ParsedSourcesToXml(IFortranObject source)
            {
                throw new NotImplementedException();
            }
        }
    }
}