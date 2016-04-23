namespace Doctran.ParsingElements.Scope
{
    using System.Collections.Generic;

    public interface IScope
    {
        Dictionary<string, IHasIdentifier> ObjectsInScope { get; }

        bool GetObjectFromIdentifier(string identifier, out IHasIdentifier obj);
    }
}