namespace Doctran.ParsingElements.Scope
{
    using System.Collections.Generic;
    using System.Linq;
    using Parsing;

    public class IdentifierObjectPair
    {
        public IdentifierObjectPair(string identifier, IHasIdentifier obj)
        {
            this.Identifier = identifier;
            this.Object = obj;
        }

        public string Identifier { get; }

        public IHasIdentifier Object { get; }
    }

    public delegate IEnumerable<IdentifierObjectPair> ScopeCalculator(IFortranObject obj);

    public abstract class Scope : IScope
    {
        private Dictionary<string, IHasIdentifier> _localIdentifiers;

        protected Scope(IFortranObject obj, ScopeCalculator getScopeItems)
        {
            this.Object = obj;
            this.GetScopeItems = getScopeItems;
        }

        public Dictionary<string, IHasIdentifier> ObjectsInScope
            => _localIdentifiers ?? (_localIdentifiers = this.GetScopeItems(this.Object).ToDictionary(obj => obj.Identifier, obj => obj.Object));

        protected IFortranObject Object { get; }

        private ScopeCalculator GetScopeItems { get; }

        public abstract bool GetObjectFromIdentifier(string identifier, out IHasIdentifier obj);
    }
}