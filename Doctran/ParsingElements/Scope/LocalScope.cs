namespace Doctran.ParsingElements.Scope
{
    using System;
    using System.Collections.Generic;
    using Parsing;

    public abstract class LocalScope : Scope
    {
        protected LocalScope(IFortranObject obj, Func<IFortranObject, IEnumerable<IHasIdentifier>> getScopeItems)
            : base(obj, getScopeItems)
        {
            this.ParentScope = (obj as IContained)?.AncestorOfType<IHasScope>().Scope;
        }

        protected IScope ParentScope { get; }

        public override bool GetObjectFromIdentifier(string identifier, out IHasIdentifier obj)
            => this.ObjectStore.TryGetValue(identifier, out obj) || (this.ParentScope?.GetObjectFromIdentifier(identifier, out obj) ?? false);
    }
}