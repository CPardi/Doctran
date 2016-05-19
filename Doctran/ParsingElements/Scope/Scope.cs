namespace Doctran.ParsingElements.Scope
{
    using System.Collections.Generic;
    using System.Linq;
    using Helper;
    using Parsing;

    public abstract class Scope : IScope
    {
        private IEnumerable<IdentifierObjectPair> _entireScope;

        protected Scope(IFortranObject obj)
        {
            this.Object = obj;
        }

        public IEnumerable<IdentifierObjectPair> EntireScope => _entireScope ?? (_entireScope = this.GetScopeItems(this.Object));

        public IErrorListener<TraverserException> ErrorListener { get; set; } = new StandardErrorListener<TraverserException>();

        public abstract ScopeCalculator GetScopeItems { get; }

        protected IFortranObject Object { get; }

        public abstract bool Exists<T>(IIdentifier identifier)
            where T : IHasIdentifier;

        /// <summary>
        ///     Given an identifier, returns the corresponding object in the scope. Will pass a <see cref="TraverserException" />
        ///     to the <see cref="ErrorListener" /> if identifier not found or if object not of type <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The expected type of the object.</typeparam>
        /// <param name="identifier">The identifier of the object.</param>
        /// <param name="obj">
        ///     The object in scope of type <typeparamref name="T" /> with identifier <paramref name="identifier" />.
        ///     If no such object exists in scope, then null is returned.
        /// </param>
        /// <returns>The object of type <typeparamref name="T" /> that has identifier <paramref name="identifier" />.</returns>
        public abstract bool GetObjectByIdentifier<T>(IIdentifier identifier, out T obj)
            where T : IHasIdentifier;

        public T GetObjectByIdentifier<T>(IIdentifier identifier)
            where T : IHasIdentifier
        {
            T obj;
            if (!this.GetObjectByIdentifier(identifier, out obj))
            {
                this.ErrorListener.Error(new TraverserException(this.Object, $"Could not find identifier '{identifier}' in scope."));
            }

            return obj;
        }

        protected bool ExistsInLocalStorage<T>(IIdentifier identifier)
            where T : IHasIdentifier
        {
            return this.EntireScope
                .Any(pair => Equals(pair.Identifier, identifier) && pair.Object is T);
        }

        protected bool GetObjectFromLocalStorage<T>(IIdentifier identifier, out T obj)
        {
            obj = this.EntireScope
                .Where(pair => Equals(pair.Identifier, identifier))
                .Select(pair => pair.Object)
                .OfType<T>()
                .SingleOrDefault();
            return obj != null;
        }
    }
}