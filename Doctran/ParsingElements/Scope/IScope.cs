namespace Doctran.ParsingElements.Scope
{
    using System.Collections.Generic;

    public interface IScope
    {
        Dictionary<IdentifierBase, IHasIdentifier> ObjectsInScope { get; }

        bool GetObjectFromIdentifier(IdentifierBase identifier, out IHasIdentifier obj);

        T GetObjectFromIdentifier<T>(IdentifierBase identifier)
            where T : class, IHasIdentifier;
    }
}