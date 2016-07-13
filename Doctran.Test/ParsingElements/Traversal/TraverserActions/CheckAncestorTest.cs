// <copyright file="CheckAncestorTest.cs" company="Christopher Pardi">
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
    using Doctran.ParsingElements.Information;
    using Doctran.ParsingElements.Traversal;
    using Doctran.Utilitys;
    using NUnit.Framework;

    [TestFixture]
    [Category("Unit")]
    public class CheckAncestorTest
    {
        [Test]
        public void InvalidFirstOfTwoClassAncestors()
        {
            {
                var action = TraverserActions.CheckAncestors<InformationValue>(new[] { typeof(InformationValue) }, new[] { typeof(TestClass) }) as ITraverserAction<InformationValue>;

                var basic = new XElement("Basic");
                var detailed = new XElement("Detailed");
                var value = new InformationValue(2, "Name", "Value", new List<FileLine>());
                var group = new InformationGroup(1, "Name", new[] { value }, new List<FileLine>());
                var root = new TestClass("child1", new[] { group });

                Assert.Throws(typeof(TraverserException), () => action.Act(value, new StandardErrorListener<TraverserException>()));
            }
        }

        [Test]
        public void InvalidFirstOfTwoInterfaceAncestors()
        {
            {
                var action = TraverserActions.CheckAncestors<InformationValue>(new[] { typeof(IHasName) }, new[] { typeof(IHasIdentifier) }) as ITraverserAction<InformationValue>;

                var basic = new XElement("Basic");
                var detailed = new XElement("Detailed");
                var value = new InformationValue(2, "Name", "Value", new List<FileLine>());
                var group = new InformationGroup(1, "Name", new[] { value }, new List<FileLine>());
                var root = new TestClass("child1", new[] { group });

                Assert.Throws(typeof(TraverserException), () => action.Act(value, new StandardErrorListener<TraverserException>()));
            }
        }

        [Test]
        public void InvalidFirstOfTwoMultipleAncestors()
        {
            {
                var action = TraverserActions.CheckAncestors<InformationValue>(
                    new[] { typeof(Description), typeof(InformationGroup) },
                    new[] { typeof(TestClass), typeof(InformationGroup) })
                    as ITraverserAction<InformationValue>;

                var basic = new XElement("Basic");
                var detailed = new XElement("Detailed");
                var value = new InformationValue(2, "Name", "Value", new List<FileLine>());
                var group = new InformationGroup(1, "Name", new[] { value }, new List<FileLine>());
                var root = new TestClass("child1", new[] { group });

                Assert.DoesNotThrow(() => action.Act(value, new StandardErrorListener<TraverserException>()));
            }
        }

        [Test]
        public void InvalidSecondOfTwoClassAncestors()
        {
            {
                var action = TraverserActions.CheckAncestors<InformationValue>(new[] { typeof(InformationGroup) }, new[] { typeof(InformationGroup) }) as ITraverserAction<InformationValue>;

                var basic = new XElement("Basic");
                var detailed = new XElement("Detailed");
                var value = new InformationValue(2, "Name", "Value", new List<FileLine>());
                var group = new InformationGroup(1, "Name", new[] { value }, new List<FileLine>());
                var root = new TestClass("child1", new[] { group });

                Assert.Throws(typeof(TraverserException), () => action.Act(value, new StandardErrorListener<TraverserException>()));
            }
        }

        [Test]
        public void InvalidSecondOfTwoMultipleAncestors()
        {
            {
                var action = TraverserActions.CheckAncestors<InformationValue>(
                    new[] { typeof(InformationValue), typeof(InformationGroup) },
                    new[] { typeof(Description), typeof(InformationGroup) })
                    as ITraverserAction<InformationValue>;

                var basic = new XElement("Basic");
                var detailed = new XElement("Detailed");
                var value = new InformationValue(2, "Name", "Value", new List<FileLine>());
                var group = new InformationGroup(1, "Name", new[] { value }, new List<FileLine>());
                var root = new TestClass("child1", new[] { group });

                Assert.Throws(typeof(TraverserException) ,() => action.Act(value, new StandardErrorListener<TraverserException>()));
            }
        }

        [Test]
        public void ValidSingleClassAncestor()
        {
            {
                var action = TraverserActions.CheckAncestors<Description>(new[] { typeof(TestClass) }) as ITraverserAction<Description>;

                var basic = new XElement("Basic");
                var detailed = new XElement("Detailed");
                var description1 = new Description(basic, detailed, new List<FileLine>());
                var child1 = new TestClass("child1", new[] { description1 });

                Assert.DoesNotThrow(() => action.Act(description1, new StandardErrorListener<TraverserException>()));
            }
        }

        [Test]
        public void ValidTwoClassAncestors()
        {
            {
                var action = TraverserActions.CheckAncestors<InformationValue>(new[] { typeof(InformationGroup) }, new[] { typeof(TestClass) }) as ITraverserAction<InformationValue>;

                var basic = new XElement("Basic");
                var detailed = new XElement("Detailed");
                var value = new InformationValue(2, "Name", "Value", new List<FileLine>());
                var group = new InformationGroup(1, "Name", new[] { value }, new List<FileLine>());
                var root = new TestClass("child1", new[] { group });

                Assert.DoesNotThrow(() => action.Act(value, new StandardErrorListener<TraverserException>()));
            }
        }

        [Test]
        public void ValidTwoInterfaceAncestors()
        {
            {
                var action = TraverserActions.CheckAncestors<InformationValue>(new[] { typeof(IInformation) }, new[] { typeof(IHasIdentifier) }) as ITraverserAction<InformationValue>;

                var basic = new XElement("Basic");
                var detailed = new XElement("Detailed");
                var value = new InformationValue(2, "Name", "Value", new List<FileLine>());
                var group = new InformationGroup(1, "Name", new[] { value }, new List<FileLine>());
                var root = new TestClass("child1", new[] { group });

                Assert.DoesNotThrow(() => action.Act(value, new StandardErrorListener<TraverserException>()));
            }
        }

        [Test]
        public void ValidTwoMultipleAncestors()
        {
            {
                var action = TraverserActions.CheckAncestors<InformationValue>(
                    new[] { typeof(InformationValue), typeof(InformationGroup) },
                    new[] { typeof(TestClass), typeof(InformationGroup) })
                    as ITraverserAction<InformationValue>;

                var basic = new XElement("Basic");
                var detailed = new XElement("Detailed");
                var value = new InformationValue(2, "Name", "Value", new List<FileLine>());
                var group = new InformationGroup(1, "Name", new[] { value }, new List<FileLine>());
                var root = new TestClass("child1", new[] { group });

                Assert.DoesNotThrow(() => action.Act(value, new StandardErrorListener<TraverserException>()));
            }
        }

        private class TestClass : Container, IContained, IHasIdentifier
        {
            public TestClass(string identifier, IEnumerable<IContained> subObjects)
                : base(subObjects)
            {
                this.Identifier = new Identifier(identifier);
            }

            public string Guid { get; } = OtherUtils.GenerateGuid();

            public IIdentifier Identifier { get; }

            public IContainer Parent { get; set; }
        }
    }
}