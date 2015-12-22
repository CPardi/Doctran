// <copyright file="Parser.Test.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Test.OptionFile
{
    using System.IO;
    using Input.OptionFile;
    using NUnit.Framework;

    [TestFixture]
    [Category("Unit")]
    public class ParserTest
    {
        private readonly string _testDir = Path.GetFullPath(@"..\..\TestFiles\InfoFile\");

        private string _originalDirectory;

        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void IncorrectFilePath()
        {
            var options = new OptionsTest();

            var parser = new Parser<OptionsTest>();
            parser.AddRecognisedOption("Name");

            parser.ParseFile("NonExistant.txt", options);
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

        [Test]
        [ExpectedException(typeof(WrongDepthException))]
        public void TestLoneSubInformation()
        {
            var options = new OptionsTest();

            var parser = new Parser<OptionsTest>();
            parser.AddRecognisedOption("Name");

            parser.ParseFile("LoneSubInformation.txt", options);
            Assert.AreEqual("TestInfo1", options.Name);
        }

        [Test(Description = "Checks that the parser works for well formed data.")]
        public void TestParsing()
        {
            var options = new OptionsTest();

            var parser = new Parser<OptionsTest>();
            parser.AddRecognisedOption("Name");

            parser.ParseFile("WellFormed.txt", options);

            Assert.AreEqual("TestInfo1", options.Name);
        }
    }
}