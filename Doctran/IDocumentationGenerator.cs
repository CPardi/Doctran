using System.Collections.Generic;
using Doctran.Parsing;

namespace Doctran
{
    public interface IDocumentationGenerator
    {
        IEnumerable<IInterfaceXElements> InterfaceXElements { get; }

        IEnumerable<IGroupXElement> ObjectGroupXElements { get; }

        IEnumerable<IObjectXElement> ObjectXElements { get; }
    }
}