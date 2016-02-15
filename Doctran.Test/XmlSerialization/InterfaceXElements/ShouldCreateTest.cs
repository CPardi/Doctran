// <copyright file="ShouldCreateTest.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Test.XmlSerialization.InterfaceXElements
{
    using Doctran.XmlSerialization;
    using NUnit.Framework;

    [TestFixture]
    public class ShouldCreateTest
    {
        [Test]
        [Description("When no predicate is specified. In this case ShouldCreate will always be true.")]
        public void NoPredicate()
        {
            var testClass = new TestClass();
            var interfaceXElements = new InterfaceXElements<ITestClass>("Name", tc => "Value");

            Assert.IsTrue(interfaceXElements.ShouldCreate(testClass));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TruePredicate(bool shouldCreate)
        {
            var testClass = new TestClass(shouldCreate);
            var interfaceXElements = new InterfaceXElements<ITestClass>("Name", tc => "Value", tc => tc.ShouldCreate);

            Assert.AreEqual(shouldCreate, interfaceXElements.ShouldCreate(testClass));
        }
    }
}