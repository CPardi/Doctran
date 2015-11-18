namespace Doctran.Parsing.BuiltIn.FortranObjects
{
    using System.Collections.Generic;
    using Helper;

    public interface IHasLines
    {
        List<FileLine> lines { get; }
    }
}