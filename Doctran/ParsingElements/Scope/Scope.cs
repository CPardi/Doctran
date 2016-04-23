namespace Doctran.ParsingElements.Scope
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Parsing;

    public abstract class Scope : IScope
    {
        private readonly IFortranObject _obj;

        private Dictionary<string, IHasIdentifier> _objectStore;

        protected Scope(IFortranObject obj, Func<IFortranObject, IEnumerable<IHasIdentifier>> getScopeItems)
        {
            _obj = obj;
            this.GetScopeItems = getScopeItems;
        }

        protected Dictionary<string, IHasIdentifier> ObjectStore
            => _objectStore ?? (_objectStore = this.GetScopeItems(_obj).ToDictionary(obj => obj.Identifier, obj => obj));

        private Func<IFortranObject, IEnumerable<IHasIdentifier>> GetScopeItems { get; }

        public abstract bool GetObjectFromIdentifier(string identifier, out IHasIdentifier obj);
    }
}