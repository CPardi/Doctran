// <copyright file="NamedDescriptionStrings.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Test.ParsingElements.FortranBlocks.NamedDescriptionBlock
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Doctran.Parsing;
    using Doctran.ParsingElements.FortranObjects;
    using NUnit.Framework;

    public class NamedDescriptionStrings
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData(
                    "!> Name - A basic description",
                    MakeAssertions("name", new XElement("Basic", "A basic description"), new XElement("Detailed", string.Empty))).SetName("A single line basic description.");

                yield return new TestCaseData(
                    "!> Name - A basic \n" +
                    "!> description",
                    MakeAssertions("name", new XElement("Basic", "A basic description"), new XElement("Detailed", string.Empty))).SetName("A multi line basic description.");

                yield return new TestCaseData(
                    "!>>Name - A detailed description",
                    MakeAssertions(
                        "name",
                        new XElement("Basic", string.Empty),
                        new XElement("Detailed", new XElement("p", "A detailed description"), "\n")))
                    .SetName("A detailed description.");

                yield return new TestCaseData(
                    "!>> Name - A detailed \n" +
                    "!>>description",
                    MakeAssertions(
                        "name",
                        new XElement("Basic", string.Empty),
                        new XElement("Detailed", new XElement("p", "A detailed description"), "\n")))
                    .SetName("A multiline detailed description.");

                yield return new TestCaseData(
                    "!>  Name - A basic description\n" +
                    "!>>A detailed description",
                    MakeAssertions(
                        "name",
                        new XElement("Basic", "A basic description"),
                        new XElement("Detailed", new XElement("p", "A detailed description"), "\n")))
                    .SetName("A basic description with a detailed description.");

                yield return new TestCaseData(
                    "!>  Name - A basic \n" +
                    "!>description\n" +
                    "!>>A detailed\n" +
                    "!>> description",
                    MakeAssertions(
                        "name",
                        new XElement("Basic", "A basic description"),
                        new XElement("Detailed", new XElement("p", "A detailed description"), "\n")))
                    .SetName("A multiline basic description with a multiline detailed description.");

                yield return new TestCaseData(
                    "!> Name - A basic \n" +
                    "!>>A detailed\n" +
                    "!>description\n" +
                    "!>> description",
                    MakeAssertions(
                        "name",
                        new XElement("Basic", "A basic description"),
                        new XElement("Detailed", new XElement("p", "A detailed description"), "\n")))
                    .SetName("A mixed up but valid description.");
            }
        }

        private static Action<IEnumerable<IFortranObject>> MakeAssertions(string linkedTo, XElement basic, XElement detailed)
        {
            return objs =>
            {
                var nl = Environment.NewLine;
                var desc = (NamedDescription)objs.Single();
                Assert.AreEqual(linkedTo, desc.LinkedTo);
                Assert.IsTrue(XNode.DeepEquals(desc.Basic, basic), $"{nl}{nl}Expected: '{basic.Value}'{nl}Actual: '{desc.Basic.Value}'");
                Assert.IsTrue(XNode.DeepEquals(desc.Detailed, detailed), $"{nl}Expected: '{detailed.Value}'{nl}Actual: '{desc.Detailed.Value}'{nl}{nl}");
            };
        }
    }
}