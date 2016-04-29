namespace Doctran.Test.ParsingElements.Identifier
{
    using Doctran.ParsingElements;
    using NUnit.Framework;

    [TestFixture]
    public class EqualsTest
    {
        [Test]
        public void EqualityTest()
        {
            var ident1 = new CaseSensitiveId("ident1");
            var ident1Dash = new CaseSensitiveId("ident1");

            Assert.IsTrue(Equals(ident1, ident1Dash));
            Assert.IsTrue(ident1.Equals(ident1Dash));
            Assert.IsTrue(ident1 == ident1Dash);
        }

        [Test]
        public void InequalityTest()
        {
            var ident1 = new CaseSensitiveId("ident1");
            var ident2 = new CaseSensitiveId("ident2");

            Assert.IsFalse(Equals(ident1, ident2));
            Assert.IsFalse(ident1.Equals(ident2));
            Assert.IsFalse(ident1 == ident2);
        }

        [Test]
        public void InequalityTest_MismatchCase()
        {
            var ident1 = new CaseSensitiveId("ident1");
            var ident2 = new CaseSensitiveId("IDENT1");

            Assert.IsFalse(Equals(ident1, ident2));
            Assert.IsFalse(ident1.Equals(ident2));
            Assert.IsFalse(ident1 == ident2);
        }
    }
}