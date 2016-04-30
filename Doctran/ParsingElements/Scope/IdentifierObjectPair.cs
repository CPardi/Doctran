namespace Doctran.ParsingElements.Scope
{
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
}