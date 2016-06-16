namespace Doctran.ParsingElements
{
    using Functional.Maybe;
    using Parsing;

    public interface IHasDescription : IFortranObject
    {
        Maybe<IDescription> Description { get; }
    }
}