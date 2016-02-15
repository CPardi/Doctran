namespace Doctran.Test.XmlSerialization.XmlGenerator
{
    using System.Xml.Linq;
    using Doctran.Utilitys;
    using Doctran.XmlSerialization;
    using NUnit.Framework;

    [TestFixture]
    public class CreateForObjectTest
    {
        [Test]
        public void OneDeepObject()
        {
            var testClass = new TestClass();

            var generator = new XmlGenerator(
                CollectionUtils.Empty<IInterfaceXElements>(),
                new[] { new ObjectXElement<TestClass>("TestClass") },
                CollectionUtils.Empty<IGroupXElement>());

            var expected = new XElement("TestClass");
            var actual = generator.CreateForObject(testClass);

            Assert.IsTrue(XNode.DeepEquals(expected, actual));
        }

        [Test]
        public void OneDeepObjectForExtension()
        {
            var testClass = new TestExtensionClass();

            var generator = new XmlGenerator(
                CollectionUtils.Empty<IInterfaceXElements>(),
                new[] { new ObjectXElement<TestClass>("TestClass") },
                CollectionUtils.Empty<IGroupXElement>());

            var expected = new XElement("TestClass");
            var actual = generator.CreateForObject(testClass);

            Assert.IsTrue(XNode.DeepEquals(expected, actual));
        }

        [Test]
        public void TwoDeepGroupObject()
        {
            var testClass = new TestContainer(true, new[] { new TestClass() });

            var generator = new XmlGenerator(
                CollectionUtils.Empty<IInterfaceXElements>(),
                new IObjectXElement[] { new ObjectXElement<TestClass>("TestClass"), new ObjectXElement<TestContainer>("TestContainer") },
                new[] { new GroupXElement<TestClass>("TestClasses") });

            var expected = new XElement("TestContainer",new XElement("TestClasses", new XElement("TestClass")));

            var actual = generator.CreateForObject(testClass);

            Assert.IsTrue(XNode.DeepEquals(expected, actual));
        }

        [Test]
        public void TwoDeepGroupObjectForExtension()
        {
            var testClass = new TestContainer(true, new[] { new TestExtensionClass() });

            var generator = new XmlGenerator(
                CollectionUtils.Empty<IInterfaceXElements>(),
                new IObjectXElement[] { new ObjectXElement<TestClass>("TestClass"), new ObjectXElement<TestContainer>("TestContainer") },
                new[] { new GroupXElement<TestClass>("TestClasses") });

            var expected = new XElement("TestContainer", new XElement("TestClasses", new XElement("TestClass")));

            var actual = generator.CreateForObject(testClass);

            Assert.IsTrue(XNode.DeepEquals(expected, actual));
        }
    }
}