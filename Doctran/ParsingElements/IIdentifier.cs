namespace Doctran.ParsingElements
{
    public interface IWord
    {
        string OriginalString { get; }

        string ToString();
    }

    public interface IIdentifier : IWord
    {
        IIdentifier CreateAlias(string newIdentifier);

        bool Equals(object obj);

        int GetHashCode();
    }
}