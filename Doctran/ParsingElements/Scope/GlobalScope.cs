namespace Doctran.ParsingElements.Scope
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Parsing;

    public class GlobalScope : Scope
    {
        public GlobalScope(IFortranObject obj, IEnumerable<ScopeCalculator> getScopeItems)
            : base(obj, CombineGlobalScopeCalculators(getScopeItems))
        {
        }

        public override bool GetObjectFromIdentifier(IdentifierBase identifier, out IHasIdentifier obj)
            => this.ObjectsInScope.TryGetValue(identifier, out obj);

        private static ScopeCalculator CombineGlobalScopeCalculators(IEnumerable<ScopeCalculator> getScopeItems)
            => o => getScopeItems.SelectMany(gsi => gsi(o));
    }
}