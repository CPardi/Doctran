// <copyright file="InformationStrings.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Test.ParsingElements.FortranBlocks.InformationBlock
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Doctran.Helper;
    using Doctran.Parsing;
    using Doctran.ParsingElements.FortranObjects;
    using NUnit.Framework;

    public class InformationStrings
    {
        public static IEnumerable TestCases
        {
            get
            {
                {
                    yield return new TestCaseData(
                        "!> Type : Information",
                        MakeValueAssertions("Type", "Information"))
                        .SetName("A single line informational element.");
                }

                {
                    yield return new TestCaseData(
                        "!> Type : Infor\n" +
                        "!>mation",
                        MakeValueAssertions("Type", "Information"))
                        .SetName("A mutliple line informational element.");
                }

                {
                    yield return new TestCaseData(
                        "!> Type : A sentence with spaces.",
                        MakeValueAssertions("Type", "A sentence with spaces."))
                        .SetName("A sentence with spaces.");
                }
            }
        }

        private static Action<IEnumerable<IFortranObject>> MakeValueAssertions(string name, string value)
        {
            return objs =>
            {
                var desc = (InformationValue)objs.Single();
                Assert.AreEqual(name, desc.Name);
                Assert.AreEqual(value, desc.Value);
            };
        }
    }
}