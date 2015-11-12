namespace Doctran.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    public class GroupXElement<TParsed> : IGroupXElement
    {
        public GroupXElement(string name)
        {
            this.Func = xElements => new XElement(name, xElements);
        }

        public GroupXElement(Func<IEnumerable<XElement>, XElement> func)
        {
            this.Func = func;
        }

        public Type ForType => typeof(TParsed);

        private Func<IEnumerable<XElement>, XElement> Func { get; }

        public XElement Create(IEnumerable<XElement> xElements)
        {
            return this.Func(xElements);
        }
    }
}