namespace Doctran.Parsing.BuiltIn.ObjectGroups
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using FortranObjects;
    using Output;

    public class NamedDescriptionGroup : ObjectGroup
    {
        public NamedDescriptionGroup()
            : base(typeof(NamedDescription)) { }

        public override XElement XEle(IEnumerable<XElement> Content)
        {
            return Content.Single();
        }
    }
}