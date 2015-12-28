//-----------------------------------------------------------------------
// <copyright file="ParserTest.cs" company="Christopher Pardi">
// Copyright © 2015 Christopher Pardi
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>
//-----------------------------------------------------------------------

namespace Doctran.Test.Input.ProjectFile
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Doctran.Input;
    using Doctran.Input.OptionsReaderCore;
    using Doctran.Input.ProjectFileCore;
    using Doctran.Parsing.Core;
    using Doctran.Utility;
    using Helper;
    using NUnit.Framework;

    [TestFixture]
    [Category("Unit")]
    internal class OptionsTest : BaseTest
    {
        private string _currentDirectory;

        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void InvalidProjectFilePath()
        {
            // Arrange and Act.
            var projFile = new ProjectFile($@"DoesNotExist.txt");
        }

        [Test]
        public void InvalidSourcePaths()
        {
            // Arrange and Act.
            Assert.Throws(typeof(ParserException), () => new ProjectFile($@"InvalidSourcePaths.txt"));
        }

        [SetUp]
        public void SetUp()
        {
            Report.SetDebugProfile();
            _currentDirectory = Environment.CurrentDirectory;
            Environment.CurrentDirectory = Paths.ProjectDirectory + @"\Input\ProjectFile\TestFiles";
        }

        [Test]
        public void SourcePaths()
        {
            // Arrange and Act.
            var expected = new List<string>
            {
                $"File1.txt",
                $"Folder2{EnvVar.Slash}File1.txt",
                $"Folder1{EnvVar.Slash}File1.txt",
                $"Folder1{EnvVar.Slash}SubFolder1{EnvVar.Slash}File1.txt",
                $"Folder1{EnvVar.Slash}SubFolder1{EnvVar.Slash}File2.txt",
                $"Folder1{EnvVar.Slash}SubFolder1{EnvVar.Slash}File3.txt"
            };

            var projFile = new ProjectFile($@"JustSourcePaths{EnvVar.Slash}JustSourcePaths.txt");
            var sources = projFile.Options.SourceFiles;

            // Assert.
            for (var i = sources.Count - 1; i >= 0; i--)
            {
                Assert.IsTrue(expected.Remove(sources[i]));
            }
            Assert.IsTrue(!expected.Any());
            Assert.NotNull(projFile.Options.CopyFiles);
            Assert.NotNull(projFile.Options.CopyAndParseFiles);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            Environment.CurrentDirectory = _currentDirectory;
        }

        [Test]
        public void InvalidUserPage()
        {
            // Arrange and Act.          
            Assert.Throws(typeof(ParserConversionException), () => new ProjectFile(@"InvalidUserPage.txt"));
        }

        [Test]
        public void UserPageAndXmlPassThrough()
        {
            // Arrange and Act.
            var expected = new XElement("MetaData",
                new XElement("UserPage", XElement.Parse("<h1>Index.md</h1>")),
                new XElement("Name", "Project 1"),
                new XElement("Author",
                    new XElement("Name",
                        new XElement("First", "Chris"),
                        new XElement("Last", "Pardi")
                        ),
                    new XElement("Email", "pardi@doctran.co.uk")
                    ));

            var projFile = new ProjectFile(@"UserPageAndXmlPassthough\project.info");
            var actual = new XElement("MetaData", projFile.Options.ProjectMetaData);

            // Assert.
            Assert.IsTrue(XNode.DeepEquals(expected, actual));
            Assert.NotNull(projFile.Options.SourceFiles);
            Assert.NotNull(projFile.Options.CopyFiles);
            Assert.NotNull(projFile.Options.CopyAndParseFiles);
        }

        [Test]
        public void XmlDefaultPassthrough()
        {
            // Arrange and Act.
            var expected = new XElement("MetaData",
                new XElement("Name", "Project 1"),
                new XElement("Author",
                    new XElement("Name",
                        new XElement("First", "Chris"),
                        new XElement("Last", "Pardi")
                        ),
                    new XElement("Email", "pardi@doctran.co.uk")
                    ));

            var projFile = new ProjectFile("JustXml.txt");
            var actual = new XElement("MetaData", projFile.Options.ProjectMetaData);

            // Assert.
            Assert.IsTrue(XNode.DeepEquals(expected, actual));
            Assert.NotNull(projFile.Options.SourceFiles);
            Assert.NotNull(projFile.Options.CopyFiles);
            Assert.NotNull(projFile.Options.CopyAndParseFiles);
        }
    }
}