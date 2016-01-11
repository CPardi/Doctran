// <copyright file="CheckNamesTest.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Test.ParsingElements.Traversal.TraverserActions
{
    using Doctran.ParsingElements;
    using Doctran.ParsingElements.Traversal;
    using NUnit.Framework;
    using Parsing;

    [TestFixture]
    [Category("Unit")]
    public class CheckNamesTest
    {
        [Test]
        public void HasName()
        {
            var testClass = new TestClass("Name");
            TraverserActions.CheckNames.Act(testClass);
            Assert.AreEqual("Name", testClass.Name);
        }

        [Test]
        [ExpectedException(typeof(TraverserException))]
        public void NoName()
        {
            var testClass = new TestClass(string.Empty);
            TraverserActions.CheckNames.Act(testClass);
        }

        private class TestClass : IHasName
        {
            public TestClass(string name)
            {
                this.Name = name;
            }

            public string Name { get; }

            public string ObjectName => "Test Class";
        }
    }
}