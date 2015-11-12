using System;

namespace Doctran.Parsing
{
    using System.Collections.Generic;
    using System.Xml.Linq;

    public interface IObjectXElement
    {
        Type ForType { get; }

        XElement Create(object from);
    }

    public interface IObjectXElement<in TParsed>
        where TParsed : FortranObject
    {
        Type ForType { get; }

        XElement Create(TParsed from);
    }

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