// <copyright file="CheckDescriptionLinkageTest.cs" company="Christopher Pardi">
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
    using Doctran.Parsing;
    using Doctran.ParsingElements;
    using Doctran.ParsingElements.FortranObjects;
    using Doctran.ParsingElements.Traversal;
    using NUnit.Framework;
    using Parsing;

    [TestFixture]
    [Category("Unit")]
    public class CheckDescriptionLinkageTest
    {
        [Test]
        public void CorrentLink()
        {
            {
                var action = TraverserActions.CheckDescriptionLinkage as ITraverserAction<NamedDescription>;

                var basic = new XElement("Basic");
                var detailed = new XElement("Detailed");
                var description = new NamedDescription("child1", basic, detailed, new List<FileLine>());
                var child1 = new TestClass("child1", new[] { description });

                Assert.DoesNotThrow(() => action.Act(description, new StandardErrorListener<TraverserException>()));
            }
        }

        [Test]
        public void InCorrentLink()
        {
            {
                var action = TraverserActions.CheckDescriptionLinkage as ITraverserAction<NamedDescription>;

                var basic = new XElement("Basic");
                var detailed = new XElement("Detailed");
                var description = new NamedDescription("wrong", basic, detailed, new List<FileLine>());
                var child1 = new TestClass("child1", new[] { description });

                Assert.Throws(typeof(TraverserException), () => action.Act(description, new StandardErrorListener<TraverserException>()));
            }
        }

        private class TestClass : Container, IContained, IHasIdentifier
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