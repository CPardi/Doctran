using System.Collections.Generic;
using Doctran.Helper;

namespace Doctran.Parsing.BuiltIn.FortranObjects
{
    public interface ISourceFile : ISource
    {
        List<FileLine> OriginalLines { get; }
        string PathAndFilename { get; }
    }
}