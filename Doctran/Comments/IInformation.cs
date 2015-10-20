namespace Doctran.Comments
{
    using System.Collections.Generic;
    using Helper;
    using Parsing;

    public interface IInformation {
        string Name { get; }
        int Depth { get; }
        List<FileLine> lines { get; }
        List<FortranObject> SubObjects { get; }
    }
}