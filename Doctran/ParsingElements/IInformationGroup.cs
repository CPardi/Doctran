namespace Doctran.ParsingElements
{
    using System.Collections.Generic;
    using Parsing;

    public interface IInformationGroup : IInformation
    {
        List<IFortranObject> SubObjects { get; }
    }
}