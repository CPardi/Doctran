//-----------------------------------------------------------------------
// <copyright file="ParseTest.cs" company="Christopher Pardi">
// Copyright © 2015 Christopher Pardi
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>
//-----------------------------------------------------------------------

namespace Doctran.Test.Input.OptionsReader.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Doctran.Input;
    using Doctran.Input.OptionsReaderCore;
    using Helper;
    using NUnit.Framework;
    using Parsing;
    using Utilitys;
    using Options = OptionsReader.Options;

    [TestFixture]
    [Category("Unit")]
    internal class ParseTest
    {
        [Test]
        public void DefaultOption()
        {
            // Arrange
            var source =
                "StringList : String Value 1\n" +
                "StringList : String Value 2\n" +
                "Default : Default Value 1\n" +
                "Default : Default Value 2\n" +
                "Default : Default Value 3";
            var sourceLines = StringUtils.ConvertToFileLineList(source);

            var options = new Options();
            var parser = new OptionsReader<Options>(1, "String option test");

            // Act
            parser.Parse(options, "Unit test", sourceLines);

            // Assert
            for (var i = 0; i < options.StringListOption.Count(); i++)
            {
                Assert.AreEqual($"String Value {i + 1}", options.StringListOption[i]);
            }
            for (var i = 0; i < options.DefaultOption.Count; i++)
            {
                Assert.AreEqual($"Default Value {i + 1}", options.DefaultOption[i]);
            }

            Assert.AreEqual(1, options.IntOption);
            Assert.Null(options.StringOption);

            Assert.Null(options.IntListOption);
            Assert.NotNull(options.StringEnumerableOption);
        }

        [Test]
        public void IntListOption()
        {
            // Arrange
            var source =
                "IntList : 23\n" +
                "IntList : 45";
            var sourceLines = StringUtils.ConvertToFileLineList(source);

            var options = new Options();
            var parser = new OptionsReader<Options>(1, "String option test");

            // Act
            parser.Parse(options, "Unit Test", sourceLines);

            // Assert
            Assert.AreEqual(23, options.IntListOption[0]);
            Assert.AreEqual(45, options.IntListOption[1]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidAttributeOptions()
        {
            // Arrange
            var source =
                "AbstractAssign : String Value 1\n" +
                "AbstractAssign : String Value 2\n" +
                "NoIList : String Value 2";
            var sourceLines = StringUtils.ConvertToFileLineList(source);

            var options = new InvalidAttributeOptions();
            var parser = new OptionsReader<InvalidAttributeOptions>(1, "Abstract assign option test");

            // Act
            parser.Parse(options, "Unit Test", sourceLines);
        }

        [Test]
        [ExpectedException(typeof(InvalidPropertyTypeException))]
        public void InvalidDefaultOption()
        {
            // Arrange
            var source =
                "Default : Default Value 1\n" +
                "Default : Default Value 2\n" +
                "Default : Default Value 3";
            var sourceLines = StringUtils.ConvertToFileLineList(source);

            var options = new InvalidDefaultOptions();
            var parser = new OptionsReader<InvalidDefaultOptions>(1, "String option test");

            // Act
            parser.Parse(options, "Unit Test", sourceLines);
        }

        [Test]
        public void InvalidIntListOption()
        {
            // Arrange
            var source =
                "IntList : Not a number\n" +
                "IntList : Another not a number\n" +
                "IntList : 12";
            var sourceLines = StringUtils.ConvertToFileLineList(source);
          
            var options = new Options();
            var parser = new OptionsReader<Options>(1, "String option test");

            var eCount = 0;
            parser.ErrorListener = new ErrorListener<OptionReaderException>(
                e => { eCount++; },
                e => { eCount++; }
                );
            parser.Parse(options, "Unit Test", sourceLines);

            Assert.IsTrue(eCount > 0);
        }

        [Test]
        [ExpectedException(typeof(InvalidAttributesException))]
        public void InvalidMutlipleAttributesOptionsList()
        {
            // Arrange
            var source =
                "IntOption : 23\n" +
                "StringOption : 23";
            var sourceLines = StringUtils.ConvertToFileLineList(source);

            var options = new InvalidMultipleAttributesOptionsList();
            var parser = new OptionsReader<InvalidMultipleAttributesOptionsList>(1, "InvalidMutlipleAttributesOptionsList");
            parser.Parse(options, "Unit Test", sourceLines);
        }

        [Test]
        public void InvalidScalarIntOption()
        {
            // Arrange
            var source =
                "ScalarInt : Not a number";
            var sourceLines = StringUtils.ConvertToFileLineList(source);

            var options = new Options();
            var parser = new OptionsReader<Options>(1, "String option test");

            var eCount = 0;
            parser.ErrorListener = new ErrorListener<OptionReaderException>(
                e => { eCount++; },
                e => { eCount++; }
                );
            parser.Parse(options, "Unit Test", sourceLines);

            Assert.IsTrue(eCount > 0);
        }

        [Test]
        public void MultiOptionList()
        {
            // Arrange
            var source =
                "MultiOptionList1 : 23\n" +
                "MultiOptionList1 : 45\n" +
                "MultiOptionList2 : 56\n" +
                "MultiOptionList2 : 67";
            var sourceLines = StringUtils.ConvertToFileLineList(source);

            var options = new MultiListOptions();
            var parser = new OptionsReader<MultiListOptions>(1, "String option test");

            // Act
            parser.Parse(options, "Unit Test", sourceLines);

            // Assert
            Assert.AreEqual(23, options.MultiOptionList[0]);
            Assert.AreEqual(45, options.MultiOptionList[1]);
            Assert.AreEqual(56, options.MultiOptionList[2]);
            Assert.AreEqual(67, options.MultiOptionList[3]);
        }

        [Test]
        public void MultiOptionListWithDefault()
        {
            // Arrange
            var source =
                "Default1 : 78\n" +
                "Default2 : 89\n" +
                "MultiOptionList1 : 23\n" +
                "MultiOptionList1 : 45\n" +
                "MultiOptionList2 : 56\n" +
                "MultiOptionList2 : 67";
            var sourceLines = StringUtils.ConvertToFileLineList(source);

            var options = new MultiListOptionsWithDefault();
            var parser = new OptionsReader<MultiListOptionsWithDefault>(1, "String option test");

            // Act
            parser.Parse(options, "Unit Test", sourceLines);

            // Assert
            Assert.AreEqual(23, options.MultiOptionListWithDefault[0]);
            Assert.AreEqual(45, options.MultiOptionListWithDefault[1]);
            Assert.AreEqual(56, options.MultiOptionListWithDefault[2]);
            Assert.AreEqual(67, options.MultiOptionListWithDefault[3]);
            Assert.AreEqual(78, options.MultiOptionListWithDefault[4]);
            Assert.AreEqual(89, options.MultiOptionListWithDefault[5]);
        }

        [Test]
        public void MultipleScalarValues()
        {
            // Arrange
            var source =
                "ScalarInt : 12\n" +
                "ScalarInt : 23";
            var sourceLines = StringUtils.ConvertToFileLineList(source);

            var options = new Options();
            var parser = new OptionsReader<Options>(1, "MultipleScalarValues test");

            
            //Act
            var eCount = 0;
            parser.ErrorListener = new ErrorListener<OptionReaderException>(
                e => { eCount++; },
                e => { eCount++; }
                );
            parser.Parse(options, "Unit Test", sourceLines);

            Assert.IsTrue(eCount > 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void NoIConvertableOptions()
        {
            // Arrange
            var source =
                "NoIConvertable : String Value 2";
            var sourceLines = StringUtils.ConvertToFileLineList(source);

            var options = new NoIConvertableOptions();
            var parser = new OptionsReader<NoIConvertableOptions>(1, "Abstract assign option test");

            // Act
            parser.Parse(options, "Unit Test", sourceLines);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void NoIListOptions()
        {
            // Arrange
            var source =
                "NoIList : String Value 2";
            var sourceLines = StringUtils.ConvertToFileLineList(source);

            var options = new NoIListOptions();
            var parser = new OptionsReader<NoIListOptions>(1, "Abstract assign option test");

            // Act
            parser.Parse(options, "Unit Test", sourceLines);
        }

        [Test]
        public void PopulatedListOption()
        {
            // Arrange
            var source =
                "IntList : 23\n" +
                "IntList : 45";
            var sourceLines = StringUtils.ConvertToFileLineList(source);

            var options = new Options();
            var parser = new OptionsReader<Options>(1, "String option test");

            // Act
            options.IntListOption = new List<int> { 21 };
            parser.Parse(options, "Unit Test", sourceLines);

            // Assert
            Assert.AreEqual(21, options.IntListOption[0]);
            Assert.AreEqual(23, options.IntListOption[1]);
            Assert.AreEqual(45, options.IntListOption[2]);
        }

        [Test]
        public void ScalarIntOption()
        {
            // Arrange
            var source =
                "ScalarInt : 23";
            var sourceLines = StringUtils.ConvertToFileLineList(source);

            var options = new Options();
            var parser = new OptionsReader<Options>(1, "String option test");

            // Act
            parser.Parse(options, "Unit Test", sourceLines);

            // Assert
            Assert.AreEqual(23, options.IntOption);
        }

        [Test]
        public void ScalarStringOption()
        {
            // Arrange
            var source =
                "ScalarString : String Value";
            var sourceLines = StringUtils.ConvertToFileLineList(source);

            var options = new Options();
            var parser = new OptionsReader<Options>(1, "String option test");

            // Act
            parser.Parse(options, "Unit Test", sourceLines);

            // Assert
            Assert.AreEqual("String Value", options.StringOption);
        }

        [Test]
        public void StringEnumerableOption()
        {
            // Arrange
            var source =
                "StringEnumerable : String Value 1\n" +
                "StringEnumerable : String Value 2";
            var sourceLines = StringUtils.ConvertToFileLineList(source);

            var options = new Options();
            var parser = new OptionsReader<Options>(1, "String option test");

            // Act
            parser.Parse(options, "Unit Test", sourceLines);

            // Assert
            Assert.AreEqual($"String Value 1", options.StringEnumerableOption.First());
            Assert.AreEqual($"String Value 2", options.StringEnumerableOption.Last());
        }

        [Test]
        public void StringListOption()
        {
            // Arrange
            var source =
                "StringList : String Value 1\n" +
                "StringList : String Value 2";
            var sourceLines = StringUtils.ConvertToFileLineList(source);

            var options = new Options();
            var parser = new OptionsReader<Options>(1, "String option test");

            // Act
            parser.Parse(options, "Unit Test", sourceLines);

            // Assert
            for (var i = 0; i < options.StringListOption.Count; i++)
            {
                Assert.AreEqual($"String Value {i + 1}", options.StringListOption[i]);
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidAttributesException))]
        public void TwiceSpecifiedOption()
        {
            // Arrange
            var source =
                "Option : 23";
            var sourceLines = StringUtils.ConvertToFileLineList(source);

            var options = new TwiceSpecifiedOptions();
            var parser = new OptionsReader<TwiceSpecifiedOptions>(1, "TwiceSpecifiedOptions");
            parser.Parse(options, "Unit Test", sourceLines);
        }

        [Test]
        [ExpectedException(typeof(InvalidAttributesException))]
        public void TwiceSpecifiedOptionsList()
        {
            // Arrange
            var source =
                "OptionList : 23";
            var sourceLines = StringUtils.ConvertToFileLineList(source);

            var options = new TwiceSpecifiedOptionsList();
            var parser = new OptionsReader<TwiceSpecifiedOptionsList>(1, "TwiceSpecifiedOptionsList");
            parser.Parse(options, "Unit Test", sourceLines);
        }
    }
}