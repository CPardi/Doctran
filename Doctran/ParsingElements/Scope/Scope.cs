namespace Doctran.ParsingElements.Scope
{
    using System.Collections.Generic;
    using System.Linq;
    using Parsing;

    public class IdentifierObjectPair
    {
        public IdentifierObjectPair(Identifier identifier, IHasIdentifier obj)
        {
            this.Identifier = identifier;
            this.Object = obj;
        }

        public Identifier Identifier { get; }

        public IHasIdentifier Object { get; }
    }

    public delegate IEnumerable<IdentifierObjectPair> ScopeCalculator(IFortranObject obj);

    public abstract class Scope : IScope
    {
        private Dictionary<Identifier, IHasIdentifier> _localIdentifiers;

        protected Scope(IFortranObject obj, ScopeCalculator getScopeItems)
        {
            this.Object = obj;
            this.GetScopeItems = getScopeItems;
        }

        public Dictionary<Identifier, IHasIdentifier> ObjectsInScope
            => _localIdentifiers ?? (_localIdentifiers = this.GetScopeItems(this.Object).ToDictionary(obj => obj.Identifier, obj => obj.Object));

        protected IFortranObject Object { get; }

        private ScopeCalculator GetScopeItems { get; }

        public abstract bool GetObjectFromIdentifier(Identifier identifier, out IHasIdentifier obj);

        /// <summary>
        /// Given an identifier, returns the corresponding object in the scope. Will return a <see cref="TraverserException"/> if identifier not found or if object not of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The expected type of the object.</typeparam>
        /// <param name="identifier">The identifier of the object.</param>
        /// <returns>The object of type <typeparamref name="T"/> that has identifier <paramref name="identifier"/>.</returns>
        /// <exception cref="TraverserException">Thrown if no object with identifier is found.</exception>
        /// <exception cref="TraverserException">Thorwn if the object is not of the expected type.</exception>
        public T GetObjectFromIdentifier<T>(Identifier identifier)
            where T : class, IHasIdentifier
        {
            IHasIdentifier obj;
            if (!this.GetObjectFromIdentifier(identifier, out obj))
            {
                throw new TraverserException(this.Object, $"Could not find '{identifier}' in scope.");
            }

            var t = obj as T;
            if (t == null)
            {
                throw new TraverserException(this.Object, $"'{identifier}' was found in scope but was not of expected type '{Names.OfType<T>()}.");
            }

            return t;
        }
    }
}