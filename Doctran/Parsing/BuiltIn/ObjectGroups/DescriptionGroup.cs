namespace Doctran.Parsing.ObjectGroups
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using FortranObjects;
    using Output;

    public class DescriptionGroup : ObjectGroup
    {
        public DescriptionGroup()
            : base(typeof(Description)) { }

        public override XElement XEle(IEnumerable<XElement> content)
        {
            return content.SingleOrDefault();
        }
    }
}