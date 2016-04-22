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

        public override bool GetObjectFromName(string name, out IHasIdentifier obj)
            => this.ObjectStore.TryGetValue(name, out obj);

        private static Func<IFortranObject, IEnumerable<IHasIdentifier>> GetAllScopeItems(IEnumerable<Func<IFortranObject, IEnumerable<IHasIdentifier>>> getScopeItems)
            => o => getScopeItems.SelectMany(gsi => gsi(o));
    }
}