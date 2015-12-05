namespace Doctran.Comments
{
    using System.Collections.Generic;
    using Helper;
    using Parsing;

    public interface IInformation {
        string Name { get; }
        int Depth { get; }
        List<FileLine> Lines { get; }
        List<IFortranObject> SubObjects { get; }
    }
}