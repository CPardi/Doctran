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

        /// <summary>
        ///     Given an identifier, returns the corresponding object in the scope. Will return a <see cref="TraverserException" />
        ///     if identifier not found or if object not of type <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The expected type of the object.</typeparam>
        /// <param name="identifier">The identifier of the object.</param>
        /// <returns>The object of type <typeparamref name="T" /> that has identifier <paramref name="identifier" />.</returns>
        /// <exception cref="TraverserException">Thrown if no object with identifier is found.</exception>
        /// <exception cref="TraverserException">Thorwn if the object is not of the expected type.</exception>
        public abstract bool GetObjectFromIdentifier<T>(Identifier identifier, out T obj)
            where T : IHasIdentifier;

        public T GetObjectFromIdentifier<T>(Identifier identifier)
            where T : IHasIdentifier
        {
            T obj;
            if (!this.GetObjectFromIdentifier(identifier, out obj))
            {
                throw new TraverserException(this.Object, $"Could not find identifier '{identifier}' in scope.");
            }

            return obj;
        }

        public bool GetObjectFromLocalStorage<T>(Identifier identifier, out T obj)
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