using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Doctran.Parsing
{
    public interface IGroupXElement
    {
        Type ForType { get; }

        XElement Create(IEnumerable<XElement> xElements);
    }
}