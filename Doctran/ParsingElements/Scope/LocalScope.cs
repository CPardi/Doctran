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

        public override bool GetObjectFromName(string name, out IHasIdentifier obj)
            => this.ObjectStore.TryGetValue(name, out obj) || this.ParentScope.GetObjectFromName(name, out obj);
    }
}