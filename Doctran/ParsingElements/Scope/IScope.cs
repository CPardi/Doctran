namespace Doctran.ParsingElements.Scope
{
    using System.Collections.Generic;

    public interface IScope
    {
        IEnumerable<IdentifierObjectPair> EntireScope { get; }

        bool Exists<T>(Identifier identifier)
            where T : IHasIdentifier;

        bool GetObjectByIdentifier<T>(Identifier identifier, out T obj)
            where T : IHasIdentifier;

        T GetObjectByIdentifier<T>(Identifier identifier)
            where T : IHasIdentifier;
    }
}