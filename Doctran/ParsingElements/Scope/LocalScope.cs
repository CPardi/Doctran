namespace Doctran.ParsingElements.Scope
{
    using System;
    using System.Collections.Generic;
    using Parsing;

    public abstract class LocalScope : Scope
    {
        protected LocalScope(IFortranObject obj, ScopeCalculator getScopeItems)
            : base(obj, getScopeItems)
        {
        }

        protected IScope ParentScope => (this.Object as IContained)?.AncestorOfType<IHasScope>().Scope;

        public override bool GetObjectFromIdentifier(string identifier, out IHasIdentifier obj)
            => this.ObjectsInScope.TryGetValue(identifier, out obj) || (this.ParentScope?.GetObjectFromIdentifier(identifier, out obj) ?? false);
    }
}