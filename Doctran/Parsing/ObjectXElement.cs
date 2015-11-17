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

        public ObjectXElement(Func<TParsed, XElement> func, Predicate<TParsed> predicate)
        {
            this.Func = func;
            this.Predicate = predicate;
        }

        public Type ForType => typeof(TParsed);

        private Func<TParsed, XElement> Func { get; }

        private Predicate<TParsed> Predicate { get; }

        public XElement Create(TParsed from)
        {
            if (this.Predicate != null && this.Predicate(from))
            {
                return null;
            }

            return this.Func(from);
        }

        XElement IObjectXElement.Create(object from) => this.Create((TParsed) from);
    }
}