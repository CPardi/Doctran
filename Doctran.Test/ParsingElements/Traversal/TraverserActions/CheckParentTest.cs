// <copyright file="CheckParentTest.cs" company="Christopher Pardi">
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

    [TestFixture]
    [Category("Unit")]
    public class CheckParentTest
    {
        [Test]
        public void InvalidInterfaceParent()
        {
            {
                var action = TraverserActions.CheckParent<Description>(typeof(IHasName)) as ITraverserAction<Description>;

                var basic = new XElement("Basic");
                var detailed = new XElement("Detailed");
                var description1 = new Description(basic, detailed, new List<FileLine>());
                var child1 = new TestClass("child1", new[] { description1 });

                Assert.Throws(typeof(TraverserException), () => action.Act(description1));
            }
        }

        [Test]
        public void ValidInterfaceParent()
        {
            {
                var action = TraverserActions.CheckParent<Description>(typeof(IHasIdentifier)) as ITraverserAction<Description>;

                var basic = new XElement("Basic");
                var detailed = new XElement("Detailed");
                var description1 = new Description(basic, detailed, new List<FileLine>());
                var child1 = new TestClass("child1", new[] { description1 });

                Assert.DoesNotThrow(() => action.Act(description1));
            }
        }

        [Test]
        public void ValidParentForConcrete()
        {
            {
                var action = TraverserActions.CheckParent<Description>(typeof(TestClass)) as ITraverserAction<Description>;

                var basic = new XElement("Basic");
                var detailed = new XElement("Detailed");
                var description1 = new Description(basic, detailed, new List<FileLine>());
                var child1 = new TestClass("child1", new[] { description1 });

                Assert.DoesNotThrow(() => action.Act(description1));
            }
        }

        [Test]
        public void ValidParentForInterface()
        {
            {
                var action = TraverserActions.CheckParent<IDescription>(typeof(TestClass)) as ITraverserAction<IDescription>;

                var basic = new XElement("Basic");
                var detailed = new XElement("Detailed");
                var description1 = new Description(basic, detailed, new List<FileLine>());
                var child1 = new TestClass("child1", new[] { description1 });

                Assert.DoesNotThrow(() => action.Act(description1));
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

            public override string ObjectName => "Test Class";

            public IContainer Parent { get; set; }
        }
    }
}