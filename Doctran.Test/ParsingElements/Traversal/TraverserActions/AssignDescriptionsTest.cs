// <copyright file="assigndescriptionstest.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Test.ParsingElements.Traversal.TraverserActions
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using Doctran.Helper;
    using Doctran.ParsingElements;
    using Doctran.ParsingElements.FortranObjects;
    using Doctran.ParsingElements.Traversal;
    using NUnit.Framework;
    using Parsing;
    using Utilitys;

    [TestFixture]
    [Category("Unit")]
    public class AssignDescriptionsTest
    {
        [Test]
        public void AssignSingleDescription()
        {
            var action = TraverserActions.AssignDescriptions as ITraverserAction<NamedDescription>;

            var basic = new XElement("Basic");
            var detailed = new XElement("Detailed");
            var description = new NamedDescription("child1", basic, detailed, new List<FileLine>() {});
            var child1 = new TestClass("child1", CollectionUtils.Empty<IContained>());
            var subObjects = new List<IContained>()
            {
                child1,
                description
            };

            var parent = new TestClass("parent", subObjects);

            action.Act(description);

            Assert.IsTrue(!parent.SubObjects.Contains(description));
            Assert.IsTrue(child1.SubObjects.Contains(description));
        }

        [Test]
        public void AssignDescriptionToMutliple()
        {
            var action = TraverserActions.AssignDescriptions as ITraverserAction<NamedDescription>;

            var basic = new XElement("Basic");
            var detailed = new XElement("Detailed");
            var description = new NamedDescription("child1", basic, detailed, new List<FileLine>() { });
            var child1 = new TestClass("child1", CollectionUtils.Empty<IContained>());
            var child1Dash = new TestClass("child1", CollectionUtils.Empty<IContained>());
            var subObjects = new List<IContained>()
            {
                child1,
                child1Dash,
                description,
            };

            var parent = new TestClass("parent", subObjects);

            action.Act(description);

            Assert.IsTrue(!parent.SubObjects.Contains(description));
            Assert.IsTrue(child1.SubObjects.Contains(description));
            Assert.IsTrue(child1Dash.SubObjects.Contains(description));
        }

        private class TestClass : Container,IContained, IHasIdentifier
        {
            public TestClass(string identifier, IEnumerable<IContained> subObjects)
                : base(subObjects)
            {
                this.Identifier = identifier;
            }

            public string Identifier { get; }

            public IContainer Parent { get; set; }
        }
    }
}