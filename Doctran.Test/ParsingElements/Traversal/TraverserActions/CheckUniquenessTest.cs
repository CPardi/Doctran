// <copyright file="CheckUniquenessTest.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Test.ParsingElements.Traversal.TraverserActions
{
    using System.Collections.Generic;
    using System.Linq;
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
    public class CheckUniquenessTest
    {
        [Test]
        public void NotUniqueDescription()
        {
            {
                var action = TraverserActions.CheckUniqueness<IDescription>() as ITraverserAction<IDescription>;

                var detailed = new XElement("Detailed");
                var description1 = new Description(new XElement("Basic1"), detailed, new List<FileLine> { new FileLine(0, string.Empty) });
                var description2 = new Description(new XElement("Basic2"), detailed, new List<FileLine> { new FileLine(1, string.Empty) });
                var child1 = new TestClass("child1", new IContained[] { description1, description2 });

                Assert.Throws(typeof(TraverserException), () => action.Act(description1));
                Assert.IsTrue(child1.SubObjects.OfType<IDescription>().Count() == 1);
            }
        }

        [Test]
        public void UniqueDescription()
        {
            {
                var action = TraverserActions.CheckUniqueness<IDescription>() as ITraverserAction<IDescription>;

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