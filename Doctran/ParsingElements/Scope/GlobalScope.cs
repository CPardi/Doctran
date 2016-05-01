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

        public override bool GetObjectByIdentifier<T>(Identifier identifier, out T obj) => this.GetObjectFromLocalStorage(identifier, out obj);

        public override bool Exists<T>(Identifier identifier)
            => this.ExistsInLocalStorage<T>(identifier);

        private static ScopeCalculator CombineGlobalScopeCalculators(IEnumerable<ScopeCalculator> getScopeItems)
            => o => getScopeItems.SelectMany(gsi => gsi(o));
    }
}