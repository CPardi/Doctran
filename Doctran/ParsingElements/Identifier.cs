namespace Doctran.ParsingElements
{
    /// <summary>
    ///     Default implementation of <see cref="IIdentifier" />. It defines a case-sensitive identifier.
    /// </summary>
    public class Identifier : IIdentifier
    {
        public Identifier(string identifier)
        {
            this.OriginalString = identifier;
        }

        public string OriginalString { get; }

        public Identifier CreateAlias(string newIdentifier)
        {
            return new Identifier(newIdentifier);
        }

        public override bool Equals(object obj)
        {
            return this.OriginalString.Equals((obj as IIdentifier)?.OriginalString);
        }

        public override int GetHashCode() => this.OriginalString.GetHashCode();

        public override string ToString() => this.OriginalString;

        IIdentifier IIdentifier.CreateAlias(string newIdentifier) => this.CreateAlias(newIdentifier);
    }
}