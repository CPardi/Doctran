namespace Doctran.ParsingElements
{
    /// <summary>
    ///     Default implementation of <see cref="IIdentifier" />.
    /// </summary>
    public class Identifier : IIdentifier
    {
        public Identifier(string identifier)
        {
            this.OriginalString = identifier;
        }

        public string OriginalString { get; }

        public static bool operator ==(Identifier obj1, IIdentifier obj2)
        {
            return obj1?.Equals(obj2) ?? false;
        }

        public static bool operator !=(Identifier obj1, IIdentifier obj2)
        {
            return !obj1?.Equals(obj2) ?? false;
        }

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