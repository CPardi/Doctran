namespace Doctran.Test.XmlSerialization.InterfaceXElements
{
    using Doctran.XmlSerialization;
    using NUnit.Framework;

    [TestFixture]
    public class InterfaceXElementsTest
    {
        [Test]
        public void UseConcreteClass()
        {
            Assert.Throws(typeof(TypeParameterException), () => new InterfaceXElements<TestClass>("Name", tc => "Value"));
        }
    }
}