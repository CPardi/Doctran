namespace Doctran.Parsing
{
    using ParsingElements;

    public interface ISource : IFortranObject, IHasIdentifier, IContainer, IContained
    {
        string Language { get; }
    }
}