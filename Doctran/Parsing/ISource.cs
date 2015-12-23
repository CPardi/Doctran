namespace Doctran.Parsing
{
    using BuiltIn.FortranObjects;

    public interface ISource : IFortranObject, IHasIdentifier
    {
        string Language { get; }
    }
}