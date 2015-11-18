namespace Doctran.Parsing.BuiltIn.FortranObjects
{
    public interface IHasName
    {
        string Name { get; }
    }

    public interface IHasValidName
    {
        string ValidName { get; }
    }
}