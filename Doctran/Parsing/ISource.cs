namespace Doctran.Parsing
{
    using ParsingElements;

    public interface ISource : IFortranObject, IHasIdentifier
    {
        string Language { get; }
    }
}