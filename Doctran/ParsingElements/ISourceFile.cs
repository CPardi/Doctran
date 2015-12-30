namespace Doctran.ParsingElements
{
    using System.Collections.Generic;
    using Helper;
    using Parsing;

    public interface ISourceFile : ISource
    {
        List<FileLine> OriginalLines { get; }
        string AbsolutePath { get; }
    }
}