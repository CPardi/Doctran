namespace Doctran.ParsingElements.Scope
{
    using System.Collections.Generic;
    using Helper;
    using Parsing;

    public interface IScope
    {
        IEnumerable<IdentifierObjectPair> EntireScope { get; }

        IErrorListener<TraverserException> ErrorListener { get; set; }

        bool Exists<T>(IIdentifier identifier)
            where T : class, IHasIdentifier;

        bool GetObjectByIdentifier<T>(IIdentifier identifier, out T obj)
            where T : class, IHasIdentifier;

        T GetObjectByIdentifier<T>(IIdentifier identifier)
            where T : class, IHasIdentifier;
    }
}