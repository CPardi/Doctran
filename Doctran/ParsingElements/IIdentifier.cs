namespace Doctran.ParsingElements
{
    public interface IIdentifier
    {
        string OriginalString { get; }

        IIdentifier CreateAlias(string newIdentifier);

        bool Equals(object obj);

        int GetHashCode();

        string ToString();
    }
}