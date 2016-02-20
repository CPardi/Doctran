// <copyright file="DescriptionStrings.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Test.ParsingElements.FortranBlocks.DescriptionBlock
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Doctran.Parsing;
    using Doctran.ParsingElements.FortranObjects;
    using NUnit.Framework;

    public class DescriptionStrings
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData(
                    "!> A basic description",
                    MakeAssertions(new XElement("Basic", new XCData("A basic description")), new XElement("Detailed", string.Empty))).SetName("A single line basic description.");

                yield return new TestCaseData(
                    "!> A basic\n" +
                    "!> description",
                    MakeAssertions(new XElement("Basic", new XCData("A basic description")), new XElement("Detailed", string.Empty))).SetName("A multi line basic description.");

                yield return new TestCaseData(
                    "!>>A detailed description",
                    MakeAssertions(
                        new XElement("Basic", new XCData(string.Empty)),
                        new XElement("Detailed", new XElement("p", "A detailed description"), "\n")))
                    .SetName("A detailed description.");

                yield return new TestCaseData(
                    "!>>A detailed \n" +
                    "!>>description",
                    MakeAssertions(
                        new XElement("Basic", new XCData(string.Empty)),
                        new XElement("Detailed", new XElement("p", "A detailed \ndescription"), "\n")))
                    .SetName("A multiline detailed description.");

                yield return new TestCaseData(
                    "!> A basic description\n" +
                    "!>>A detailed description",
                    MakeAssertions(
                        new XElement("Basic", new XCData("A basic description")),
                        new XElement("Detailed", new XElement("p", "A detailed description"), "\n")))
                    .SetName("A basic description with a detailed description.");

                yield return new TestCaseData(
                    "!> A basic\n" +
                    "!> description\n" +
                    "!>>A detailed\n" +
                    "!>> description",
                    MakeAssertions(
                        new XElement("Basic", new XCData("A basic description")),
                        new XElement("Detailed", new XElement("p", "A detailed\n description"), "\n")))
                    .SetName("A multiline basic description with a multiline detailed description.");

                yield return new TestCaseData(
                    "!> A basic\n" +
                    "!>>A detailed\n" +
                    "!>description\n" +
                    "!>> description",
                    MakeAssertions(
                        new XElement("Basic", new XCData("A basic description")),
                        new XElement("Detailed", new XElement("p", "A detailed\n description"), "\n")))
                    .SetName("An mixed up but valid description.");
            }
        }

        private static Action<IEnumerable<IFortranObject>> MakeAssertions(XElement basic, XElement detailed)
        {
            return objs =>
            {
                var nl = Environment.NewLine;
                var desc = (Description)objs.Single();
                Assert.IsTrue(XNode.DeepEquals(desc.Basic, basic), $"{nl}{nl}Expected: '{basic.Value}'{nl}Actual:   '{desc.Basic.Value}'");
                Assert.IsTrue(XNode.DeepEquals(desc.Detailed, detailed), $"{nl}Expected: '{detailed.ToString().Replace("\n", @"\n")}'{nl}Actual:   '{desc.Detailed.ToString().Replace("\n", @"\n")}'{nl}{nl}");
            };
        }
    }
}