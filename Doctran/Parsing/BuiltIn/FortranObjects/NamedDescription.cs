namespace Doctran.Parsing.BuiltIn.FortranObjects
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using Helper;

    public class NamedDescription : FortranObject
    {
        public string LinkedTo { get; }

        public NamedDescription(string linkedTo, XElement basic, XElement detailed, List<FileLine> lines)
            : base(lines)
        {
            this.LinkedTo = linkedTo;
            this.Basic = basic;
            this.Detailed = detailed;
        }

        public XElement Basic { get; }

        public XElement Detailed { get; }
    }
}