namespace Doctran.Parsing.FortranObjects
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Comments;
    using Helper;
    using Parsing;

    public class XInformation : XFortranObject, IInformation
    {
        public string Value { get; private set; }
        public int Depth { get; private set; }

        public XInformation(int depth, string name, string value, List<FileLine> lines)
            : base(name, lines)
        {
            this.Name = name;
            this.Value = value;
            this.Depth = depth;
        }

        public XInformation(int depth, string name, string value, IEnumerable<FortranObject> sub_objects, List<FileLine> lines)
            : base(name, sub_objects, lines)
        {
            this.Name = name;
            this.Value = value;
            this.Depth = depth;

        }

        protected override string GetIdentifier()
        {
            return "Information(" + this.XElementName + ")";
        }

        public override XElement XEle()
        {
            return (string.IsNullOrEmpty(Value)
                ?
                new XElement(this.XElementName, this.SubObjects.Select(sinfo => (sinfo as XInformation).XEle()))
                :
                XElement.Parse("<" + this.XElementName + ">" + this.Value + "</" + this.XElementName + ">")
                );
        }
    }
}