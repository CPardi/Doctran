namespace Doctran.ParsingElements.Scope
{
    using System.Collections.Generic;

    public interface IScope
    {
        IEnumerable<IdentifierObjectPair> EntireScope { get; }

        bool Exists<T>(IIdentifier identifier)
            where T : IHasIdentifier;

        bool GetObjectByIdentifier<T>(IIdentifier identifier, out T obj)
            where T : IHasIdentifier;

        T GetObjectByIdentifier<T>(IIdentifier identifier)
            where T : IHasIdentifier;
    }
}