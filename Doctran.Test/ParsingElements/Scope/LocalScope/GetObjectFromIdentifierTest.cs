namespace Doctran.Test.ParsingElements.Scope.LocalScope
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
            var unit1 = new IdentifiableUnit(null, "Unit1");
            var unit2 = new IdentifiableUnit(null, "Unit2");
            var inScopeArray = new[] { new IdentifierObjectPair(unit1.Identifier, unit1), new IdentifierObjectPair(unit2.Identifier, unit2) };

            ScopeCalculator getLocalScope = p => inScopeArray;

            var gs = new MyLocalScope(null, getLocalScope);

            IdentifiableUnit obj;
            Assert.IsTrue(gs.GetObjectFromIdentifier(new CaseSensitiveId("Unit1"), out obj), "Cound not find 'Unit1'.");
            Assert.AreEqual(unit1, obj);

            Assert.IsFalse(gs.GetObjectFromIdentifier(new CaseSensitiveId("Unit3"), out obj), "Found non-existant object 'Unit3'.");
        }

        [Test]
        public void SetAndRetrieveFromParentScope()
        {
            // Top level object.
            var unit0 = new ScopingUnit();

            // Set global scope.
            var unit3 = new IdentifiableUnit(unit0, "Unit3");
            var unit4 = new IdentifiableUnit(unit0, "Unit4");
            unit0.InScope = new[] { new IdentifierObjectPair(unit3.Identifier, unit3), new IdentifierObjectPair(unit4.Identifier, unit4) };

            // Set local scope
            var unit1 = new IdentifiableUnit(unit0, "Unit1");
            var unit2 = new IdentifiableUnit(unit0, "Unit2");

            // Add subobjects to unit1
            unit0.AddSubObjects(new[] { unit1, unit2 });

            // Create local scope
            ScopeCalculator getLocalScope = p => new [] { new IdentifierObjectPair(unit1.Identifier, unit1), new IdentifierObjectPair(unit2.Identifier, unit2) };
            var myLocalScope = new MyLocalScope(unit1, getLocalScope);

            // Check object from local scope.
            IHasIdentifier obj1;
            Assert.IsTrue(myLocalScope.GetObjectFromIdentifier(new CaseSensitiveId("Unit1"), out obj1), "Cound not find 'Unit1'.");
            Assert.AreEqual(unit1, obj1);

            // Check object from global scope.
            IHasIdentifier obj3;
            Assert.IsTrue(myLocalScope.GetObjectFromIdentifier(new CaseSensitiveId("Unit3"), out obj3), "Cound not find 'Unit3'.");
            Assert.AreEqual(unit3, obj3);

            // Check for non-existant object.
            IHasIdentifier obj5;
            Assert.IsFalse(myLocalScope.GetObjectFromIdentifier(new CaseSensitiveId("Unit5"), out obj5), "Found non-existing object 'Unit5'.");
        }

        private class IdentifiableUnit : IHasIdentifier, IContained
        {
            public IdentifiableUnit(IContainer parent, string identifier)
            {
                this.Parent = parent;
                this.Identifier = new CaseSensitiveId(identifier);
            }

            public Identifier Identifier { get; }

            public string ObjectName => "Identifiable Unit";

            public IContainer Parent { get; set; }
        }

        private class MyLocalScope : LocalScope
        {
            public MyLocalScope(IFortranObject obj, ScopeCalculator getScopeItems)
                : base(obj, getScopeItems)
            {
            }
        }

        private class ScopingUnit : Container, IHasScope
        {
            public ScopingUnit()
                : base(new IContained[] { })
            {
            }

            public IEnumerable<IdentifierObjectPair> InScope { get; set; }

            public IScope Scope => new GlobalScope(
                this,
                new ScopeCalculator[] { unit => this.InScope });
        }
    }
}