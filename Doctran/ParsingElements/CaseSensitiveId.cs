namespace Doctran.ParsingElements
{
    /// <summary>
    ///     Default implementation of <see cref="Identifier" />.
    /// </summary>
    public class CaseSensitiveId : Identifier
    {
        public CaseSensitiveId(string identifier)
            : base(identifier)
        {
        }

        public override bool Equals(object obj)
        {
            return this.OriginalString.Equals((obj as Identifier)?.OriginalString);
        }

        public override int GetHashCode() => this.OriginalString.GetHashCode();

        public override string ToString() => this.OriginalString;
    }
}