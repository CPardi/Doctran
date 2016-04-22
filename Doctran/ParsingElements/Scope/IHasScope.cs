namespace Doctran.ParsingElements.Scope
{
    using Parsing;

    public interface IHasScope : IFortranObject
    {
        IScope Scope { get; }
    }
}