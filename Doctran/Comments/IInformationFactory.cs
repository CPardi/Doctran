namespace Doctran.Comments
{
    using System.Collections.Generic;
    using Helper;
    using Parsing;

    public interface IInformationFactory
    {
        IEnumerable<IInformation> Create(int depth, string value, IEnumerable<IFortranObject> subObjects, List<FileLine> lines);
    }
}