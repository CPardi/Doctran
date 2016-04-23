namespace Doctran.Test.ParsingElements.Scope.GlobalScope
{
    using System;
    using System.Collections.Generic;
    using Doctran.Parsing;
    using Doctran.ParsingElements;
    using Doctran.ParsingElements.Scope;
    using NUnit.Framework;

    [TestFixture]
    public class GetObjectFromIdentifierTest
    {
        [Test]
        public void SetAndRetrieve()
        {
            var unit1 = new ScopingUnit("Unit1");
            var unit2 = new ScopingUnit("Unit2");
            var hasIdentifiers = new [] { new IdentifierObjectPair(unit1.Identifier, unit1), new IdentifierObjectPair(unit2.Identifier, unit2) };

            var getLocalScope = new ScopeCalculator[] { p => hasIdentifiers };

            var gs = new GlobalScope(null, getLocalScope);

            IHasIdentifier obj;
            gs.GetObjectFromIdentifier("Unit1", out obj);
            Assert.AreEqual(unit1, obj);

            Assert.IsFalse(gs.GetObjectFromIdentifier("Unit3", out obj), "Found non-existing object 'Unit3'.");
        }

        private class ScopingUnit : IHasIdentifier
        {
            public ScopingUnit(string identifier)
            {
                this.Identifier = identifier;
            }

            public string Identifier { get; }

            public string ObjectName => "Scoping Unit";
        }
    }
}