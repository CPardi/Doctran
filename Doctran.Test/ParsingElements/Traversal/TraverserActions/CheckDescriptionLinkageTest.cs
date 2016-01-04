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
    public class CheckDescriptionLinkageTest
    {
        [Test]
        public void CorrentLink()
        {
            {
                var action = TraverserActions.CheckDescriptionLinkage as ITraverserAction<NamedDescription>;

                var basic = new XElement("Basic");
                var detailed = new XElement("Detailed");
                var description = new NamedDescription("child1", basic, detailed, new List<FileLine>() { });
                var child1 = new TestClass("child1", new[] { description });

                Assert.DoesNotThrow(() => action.Act(description));
            }
        }

        [Test]
        public void InCorrentLink()
        {
            {
                var action = TraverserActions.CheckDescriptionLinkage as ITraverserAction<NamedDescription>;

                var basic = new XElement("Basic");
                var detailed = new XElement("Detailed");
                var description = new NamedDescription("wrong", basic, detailed, new List<FileLine>() { });
                var child1 = new TestClass("child1", new[] { description });

                Assert.Throws(typeof(TraverserException), () => action.Act(description));
            }
        }

        private class TestClass : Container, IContained, IHasIdentifier
        {
            public TestClass(string identifier, IEnumerable<IContained> subObjects)
                : base(subObjects)
            {
                this.Identifier = identifier;
            }

            public override string ObjectName => "Test Class";

            public string Identifier { get; }

            public IContainer Parent { get; set; }
        }
    }
}