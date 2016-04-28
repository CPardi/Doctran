namespace Doctran.ParsingElements
{
    /// <summary>
    ///     Default implementation of <see cref="IdentifierBase" />.
    /// </summary>
    public class Identifier : IdentifierBase
    {
        public Identifier(string identifier)
            : base(identifier)
        {
        }

        public override bool Equals(object obj)
        {
            return this.OriginalString.Equals((obj as IdentifierBase)?.OriginalString);
        }

        public override int GetHashCode() => this.OriginalString.GetHashCode();

        public override string ToString() => this.OriginalString;
    }
}