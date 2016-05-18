namespace Doctran.ParsingElements.Scope
{
    using System.Collections.Generic;
    using System.Linq;
    using Parsing;

    public class GlobalScope : Scope
    {
        public GlobalScope(IFortranObject obj, IEnumerable<ScopeCalculator> getScopeItems)
            : base(obj)
        {
            this.GetScopeItems = CombineGlobalScopeCalculators(getScopeItems);
        }

        public override ScopeCalculator GetScopeItems { get; }

        public override bool Exists<T>(IIdentifier identifier)
            => this.ExistsInLocalStorage<T>(identifier);

        public override bool GetObjectByIdentifier<T>(IIdentifier identifier, out T obj) => this.GetObjectFromLocalStorage(identifier, out obj);

        private static ScopeCalculator CombineGlobalScopeCalculators(IEnumerable<ScopeCalculator> getScopeItems)
            => o => getScopeItems.SelectMany(gsi => gsi(o));
    }
}