namespace Doctran.ParsingElements.Scope
{
    using System;
    using Parsing;
    using Functional.Maybe;

    public abstract class LocalScope : Scope
    {
        protected LocalScope(IFortranObject obj)
            : base(obj)
        {
        }

        protected IScope ParentScope => (this.Object as IContained)?.AncestorOfType<IHasScope>().Select(ihs => ihs.Scope).OrElseDefault();

        public override bool Exists<T>(IIdentifier identifier)
         => this.ExistsInLocalStorage<T>(identifier) || (this.ParentScope?.Exists<T>(identifier) ?? false);

        public override bool GetObjectByIdentifier<T>(IIdentifier identifier, out T obj)
            => this.GetObjectFromLocalStorage(identifier, out obj) || (this.ParentScope?.GetObjectByIdentifier(identifier, out obj) ?? false);
    }
}