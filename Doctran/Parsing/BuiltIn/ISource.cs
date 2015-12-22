namespace Doctran.Parsing.BuiltIn.FortranObjects
{
    public interface ISource : IFortranObject, IHasIdentifier
    {
        string Language { get; }
    }
}