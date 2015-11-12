namespace Doctran.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    public class InterfaceXElements<TParsed> : IInterfaceXElements, IInterfaceXElements<TParsed>
    {
        public InterfaceXElements(Func<TParsed, IEnumerable<XElement>> func)
        {
            if (!typeof(TParsed).IsInterface)
            {
                throw new TypeParameterException($"'{nameof(TParsed)}' must be an interface, not an implementation. The specified " +
                                                 $"generic type parameter is the class '{typeof(TParsed).Name}'.");
            }

            this.Func = func;
        }

        public Type ForType => typeof(TParsed);

        private Func<TParsed, IEnumerable<XElement>> Func { get; }

        public IEnumerable<XElement> Create(TParsed from) => this.Func(from);

        IEnumerable<XElement> IInterfaceXElements.Create(object from) => this.Create((TParsed)from);
    }

    public class TypeParameterException : ApplicationException
    {
        public TypeParameterException(string message) : base(message)
        {
        }
    }
}