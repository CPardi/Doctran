namespace Doctran.ParsingElements.Scope
{
    using Parsing;

    public interface IHasScope : IFortranObject
    {
        IScope Scope { get; }
    }

    public interface IHasScope<out TScope> : IHasScope
        where TScope : IScope
    {
        new TScope Scope { get; }
    }
}