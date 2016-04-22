// <copyright file="CreateTest.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Test.XmlSerialization.InterfaceXElements
{
    using System.Xml.Linq;
    using Doctran.Utilitys;
    using Doctran.XmlSerialization;
    using NUnit.Framework;

    [TestFixture]
    public class CreateTest
    {
        [Test]
        public void NameValue()
        {
            var testClass = new TestClass();
            var interfaceXElements = new InterfaceXElements<ITestClass>("Name", tc => "Value");

            Assert.IsTrue(
                XNode.DeepEquals(
                    new XElement("Root", new object[] { new XElement("Name", "Value") }),
                    new XElement("Root", interfaceXElements.Create(testClass))));
        }

        [Test]
        public void CreateFromFunc()
        {
            var testClass = new TestClass();
            var interfaceXElements = new InterfaceXElements<ITestClass>(tc => new XElement("Name", "Value").Singlet());

            Assert.IsTrue(
                XNode.DeepEquals(
                    new XElement("Root", new object[] { new XElement("Name", "Value") }),
                    new XElement("Root", interfaceXElements.Create(testClass))));
        }
    }
}