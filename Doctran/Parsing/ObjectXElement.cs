namespace Doctran.Parsing
{
    using System;
    using System.Xml.Linq;

    public class ObjectXElement<TParsed> : IObjectXElement, IObjectXElement<TParsed>
        where TParsed : FortranObject
    {
        public ObjectXElement(string name)
        {
            this.Func = from => new XElement(name);
        }

        public ObjectXElement(Func<TParsed, XElement> func)
        {
            this.Func = func;
        }

        public Type ForType => typeof(TParsed);

        private Func<TParsed, XElement> Func { get; }

        public XElement Create(TParsed from) => this.Func(from);

        XElement IObjectXElement.Create(object from) => this.Create((TParsed)from);
    }
}