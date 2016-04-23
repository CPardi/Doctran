namespace Doctran.ParsingElements.Scope
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Parsing;

    public class GlobalScope : Scope
    {
        public GlobalScope(IFortranObject obj, IEnumerable<Func<IFortranObject, IEnumerable<IHasIdentifier>>> getScopeItems)
            : base(obj, GetAllScopeItems(getScopeItems))
        {
        }

        public override bool GetObjectFromIdentifier(string identifier, out IHasIdentifier obj)
            => this.ObjectStore.TryGetValue(identifier, out obj);

        private static Func<IFortranObject, IEnumerable<IHasIdentifier>> GetAllScopeItems(IEnumerable<Func<IFortranObject, IEnumerable<IHasIdentifier>>> getScopeItems)
            => o => getScopeItems.SelectMany(gsi => gsi(o));
    }
}