// <copyright file="ToRandomCaseTest.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Test.Utilitys.StringUtils
{
    using Doctran.Utilitys;
    using NUnit.Framework;

    [TestFixture]
    public class ToRandomCaseTest
    {
        [Test]
        [TestCase("an all lower case statement.")]
        [TestCase("AN ALL UPPER CASE STATEMENT.")]
        [TestCase("A Mixed Case Statement.")]
        public void Test(string testString)
        {
            var randomizedString = testString.ToRandomCase();

            Assert.AreNotEqual(testString, randomizedString, "Possible but unlikely to be equal.");
            Assert.AreNotEqual(testString.ToLower(), randomizedString, "Possible but unlikely to be equal.");
            Assert.AreNotEqual(testString.ToUpper(), randomizedString, "Possible but unlikely to be equal.");
        }
    }
}