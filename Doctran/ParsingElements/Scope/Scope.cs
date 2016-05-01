namespace Doctran.ParsingElements.Scope
{
    using System.Collections.Generic;
    using System.Linq;
    using Parsing;

    public abstract class Scope : IScope
    {
        private IEnumerable<IdentifierObjectPair> _entireScope;

        protected Scope(IFortranObject obj, ScopeCalculator getScopeItems)
        {
            this.Object = obj;
            this.GetScopeItems = getScopeItems;
        }

        public IEnumerable<IdentifierObjectPair> EntireScope => _entireScope ?? (_entireScope = this.GetScopeItems(this.Object));

        protected IFortranObject Object { get; }

        private ScopeCalculator GetScopeItems { get; }

        public abstract bool Exists<T>(Identifier identifier)
            where T : IHasIdentifier;

        /// <summary>
        ///     Given an identifier, returns the corresponding object in the scope. Will return a <see cref="TraverserException" />
        ///     if identifier not found or if object not of type <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The expected type of the object.</typeparam>
        /// <param name="identifier">The identifier of the object.</param>
        /// <param name="obj">
        ///     The object in scope of type <typeparamref name="T" /> with identifier <paramref name="identifier" />.
        ///     If no such object exists in scope, then null is returned.
        /// </param>
        /// <returns>The object of type <typeparamref name="T" /> that has identifier <paramref name="identifier" />.</returns>
        /// <exception cref="TraverserException">Thrown if no object with identifier is found.</exception>
        public abstract bool GetObjectByIdentifier<T>(Identifier identifier, out T obj)
            where T : IHasIdentifier;

        public T GetObjectByIdentifier<T>(Identifier identifier)
            where T : IHasIdentifier
        {
            T obj;
            if (!this.GetObjectByIdentifier(identifier, out obj))
            {
                throw new TraverserException(this.Object, $"Could not find identifier '{identifier}' in scope.");
            }

            return obj;
        }

        protected bool ExistsInLocalStorage<T>(Identifier identifier)
            where T : IHasIdentifier
        {
            return this.EntireScope
                .Any(pair => pair.Identifier == identifier && pair.Object is T);
        }

        protected bool GetObjectFromLocalStorage<T>(Identifier identifier, out T obj)
        {
            obj = this.EntireScope
                .Where(pair => pair.Identifier == identifier)
                .Select(pair => pair.Object)
                .OfType<T>()
                .SingleOrDefault();
            return obj != null;
        }
    }
}