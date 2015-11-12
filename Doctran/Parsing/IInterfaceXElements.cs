namespace Doctran.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    public interface IInterfaceXElements
    {
        Type ForType { get; }

        IEnumerable<XElement> Create(object from);
    }

    public interface IInterfaceXElements<in TParsed>
    {
        Type ForType { get; }

        IEnumerable<XElement> Create(TParsed from);
    }
}