namespace Doctran.ParsingElements.Scope
{
    using Parsing;

    public abstract class LocalScope : Scope
    {
        protected LocalScope(IFortranObject obj, ScopeCalculator getScopeItems)
            : base(obj, getScopeItems)
        {
        }

        protected IScope ParentScope => (this.Object as IContained)?.AncestorOfType<IHasScope>()?.Scope;

        public override bool GetObjectFromIdentifier<T>(Identifier identifier, out T obj) 
            => this.GetObjectFromLocalStorage<T>(identifier, out obj) || (this.ParentScope?.GetObjectFromIdentifier(identifier, out obj) ?? false);
    }
}