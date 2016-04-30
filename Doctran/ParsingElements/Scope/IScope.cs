namespace Doctran.ParsingElements.Scope
{
    using System.Collections.Generic;

    public interface IScope
    {
        IEnumerable<IdentifierObjectPair> EntireScope { get; }

        bool GetObjectFromIdentifier<T>(Identifier identifier, out T obj)
            where T : IHasIdentifier;

        T GetObjectFromIdentifier<T>(Identifier identifier)
            where T : IHasIdentifier;
    }
}