﻿// <copyright file="ShouldCreateTest.cs" company="Christopher Pardi">
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
    public class ForTypeTest
    {
        [Test]
        public void EqualsGenericType()
        {
            var interfaceXElements = new InterfaceXElements<ITestClass>("Name", tc => "Value");

            Assert.AreEqual(typeof(ITestClass), interfaceXElements.ForType);
        }
    }
}