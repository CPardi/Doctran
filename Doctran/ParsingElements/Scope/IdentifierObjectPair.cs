namespace Doctran.ParsingElements.Scope
{
    public class IdentifierObjectPair
    {
        public IdentifierObjectPair(IIdentifier identifier, IHasIdentifier obj)
        {
            this.Identifier = identifier;
            this.Object = obj;
        }

        public IIdentifier Identifier { get; }

        public IHasIdentifier Object { get; }
    }
}