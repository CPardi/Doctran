namespace Doctran.ParsingElements.Scope
{
    using System.Collections.Generic;

    public interface IScope
    {
        Dictionary<Identifier, IHasIdentifier> ObjectsInScope { get; }

        bool GetObjectFromIdentifier(Identifier identifier, out IHasIdentifier obj);

        T GetObjectFromIdentifier<T>(Identifier identifier)
            where T : class, IHasIdentifier;
    }
}