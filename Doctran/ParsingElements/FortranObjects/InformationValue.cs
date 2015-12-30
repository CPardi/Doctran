namespace Doctran.ParsingElements.FortranObjects
{
    using System.Collections.Generic;
    using Helper;
    using Parsing;

    public class InformationValue : FortranObject, IInformationValue
    {
        public InformationValue(int depth, string name, string value, List<FileLine> lines)
            : base(lines) 
        {
            this.Name = name;
            this.Value = value;
            this.Depth = depth;
        }

        public int Depth { get; }

        public string Name { get; }

        public string Value { get; }        
    }
}