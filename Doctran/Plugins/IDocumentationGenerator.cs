namespace Doctran.Plugins
{
    using System.Collections.Generic;
    using Parsing;

    public interface IDocumentationGenerator
    {
        IEnumerable<IInterfaceXElements> InterfaceXElements { get; }

        IEnumerable<IGroupXElement> ObjectGroupXElements { get; }

        IEnumerable<IObjectXElement> ObjectXElements { get; }
    }
}