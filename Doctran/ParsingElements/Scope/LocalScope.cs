namespace Doctran.ParsingElements.Scope
{
    using Parsing;

    public abstract class LocalScope : Scope
    {
        protected LocalScope(IFortranObject obj, ScopeCalculator getScopeItems)
            : base(obj, getScopeItems)
        {
        }

        protected IScope ParentScope
        {
            get
            {
                var parentScope = (this.Object as IContained)?.AncestorOfType<IHasScope>()?.Scope;
                return parentScope;
            }
        }

        public override bool GetObjectFromIdentifier(string identifier, out IHasIdentifier obj)
            => this.ObjectsInScope.TryGetValue(identifier, out obj) || (this.ParentScope?.GetObjectFromIdentifier(identifier, out obj) ?? false);
    }
}