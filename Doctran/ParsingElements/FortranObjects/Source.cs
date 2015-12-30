namespace Doctran.ParsingElements.FortranObjects
{
    using System.Collections.Generic;
    using Helper;
    using Parsing;

    public class Source : FortranObject, ISource
    {
        public Source(string language, IEnumerable<IFortranObject> subObjects, List<FileLine> lines)
            :base(subObjects, lines)
        {
            this.Language = language;
        }

        public string Language { get; }

        public string Identifier => $"{this.Language} source";
    }
}