﻿//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.


using Doctran;
using NUnit.Framework;
using System.IO;

namespace Doctran.Test.OptionFile
{
    using Input.OptionFile;

    [TestFixture]
    [Category("Unit")]
    public class ParserTest
    {
        private string _originalDirectory;
        private readonly string _testDir = Path.GetFullPath(@"..\..\TestFiles\InfoFile\");

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

        [Test(Description = "Checks that the parser works for well formed data.")]
        public void TestParsing()
        {
            OptionsTest options = new OptionsTest();

            Parser<OptionsTest> parser = new Parser<OptionsTest>();
            parser.AddRecognisedOption("Name");

            parser.ParseFile("WellFormed.txt", options);

            Assert.AreEqual("TestInfo1", options.Name);
        }

        [Test]
        [ExpectedException(typeof(WrongDepthException))]
        public void TestLoneSubInformation()
        {
            OptionsTest options = new OptionsTest();

            Parser<OptionsTest> parser = new Parser<OptionsTest>();
            parser.AddRecognisedOption("Name");

            parser.ParseFile("LoneSubInformation.txt", options);
            Assert.AreEqual("TestInfo1", options.Name);
        }

        [Test]
        [ExpectedException(typeof(IOException))]
        public void IncorrectFilePath()
        {
            var options = new OptionsTest();

            var parser = new Parser<OptionsTest>();
            parser.AddRecognisedOption("Name");

            parser.ParseFile("NonExistant.txt", options);
            
        }
    }
}
