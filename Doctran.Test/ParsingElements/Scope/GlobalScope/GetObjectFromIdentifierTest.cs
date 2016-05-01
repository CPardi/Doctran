namespace Doctran.Test.ParsingElements.Scope.GlobalScope
{
    using System.Collections.Generic;
    using Doctran.ParsingElements;
    using Doctran.ParsingElements.Scope;
    using NUnit.Framework;

    [TestFixture]
    public class GetObjectFromIdentifierTest
    {
        private static readonly ScopingUnit Unit1 = new ScopingUnit("Unit1");

        private static readonly ScopingUnit Unit2 = new ScopingUnit("Unit2");

        private static readonly ScopingUnit2 Unit2Dash = new ScopingUnit2("Unit2");

        private static readonly IEnumerable<IdentifierObjectPair> HasIdentifiers = new[]
        {
            new IdentifierObjectPair(Unit1.Identifier, Unit1),
            new IdentifierObjectPair(Unit2.Identifier, Unit2),
            new IdentifierObjectPair(Unit2.Identifier, Unit2Dash)
        };

        private static readonly ScopeCalculator[] GetLocalScope = { p => HasIdentifiers };

        private static readonly GlobalScope Gs = new GlobalScope(null, GetLocalScope);

        [Test]
        public void RetrieveByInheritedType()
        {
            IHasIdentifier obj;
            Assert.IsTrue(Gs.GetObjectByIdentifier(new CaseSensitiveId("Unit1"), out obj));
            Assert.AreEqual(Unit1, obj);
        }

        [Test]
        public void RetrieveByTypeDifference()
        {
            ScopingUnit2 obj;
            Assert.IsTrue(Gs.GetObjectByIdentifier(new CaseSensitiveId("Unit2"), out obj));
            Assert.AreEqual(Unit2Dash, obj);
        }

        [Test]
        public void RetrieveExisting()
        {
            ScopingUnit obj;
            Assert.IsTrue(Gs.GetObjectByIdentifier(new CaseSensitiveId("Unit1"), out obj));
            Assert.AreEqual(Unit1, obj);
        }

        [Test]
        public void TryToRetrieveNonExisting()
        {
            ScopingUnit obj;
            Assert.IsFalse(Gs.GetObjectByIdentifier(new CaseSensitiveId("Unit3"), out obj), "Found non-existing object 'Unit3'.");
        }

        private class ScopingUnit : IHasIdentifier
        {
            public ScopingUnit(string identifier)
            {
                this.Identifier = new CaseSensitiveId(identifier);
            }

            public Identifier Identifier { get; }

            public string ObjectName => "Scoping Unit";
        }

        private class ScopingUnit2 : IHasIdentifier
        {
            public ScopingUnit2(string identifier)
            {
                this.Identifier = new CaseSensitiveId(identifier);
            }

            public Identifier Identifier { get; }

            public string ObjectName => "Scoping Unit";
        }
    }
}