namespace Doctran.Parsing.BuiltIn.FortranObjects
{
    public interface ISource : IFortranObject
    {
        string Language { get; }
    }
}