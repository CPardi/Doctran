namespace Doctran.Plugins
{
    using System.Collections.Generic;
    using Parsing;

    public static class DocumentationElementManager
    {
        private static readonly List<IInterfaceXElements> InterfaceXElements = new List<IInterfaceXElements>();
        private static readonly List<IObjectXElement> ObjectXElements = new List<IObjectXElement>();
        private static readonly List<IGroupXElement> GroupXElements = new List<IGroupXElement>();


        public static void RegisterNewInterfaceXElements(IEnumerable<IInterfaceXElements> interfaceXElements)
        {
            InterfaceXElements.AddRange(interfaceXElements);
        }

        public static void RegisterNewObjectXElements(IEnumerable<IObjectXElement> objectXElements)
        {
            ObjectXElements.AddRange(objectXElements);
        }

        public static void RegisterNewGroupXElements(IEnumerable<IGroupXElement> groupXElements)
        {
            GroupXElements.AddRange(groupXElements);
        }

        public static IEnumerable<IInterfaceXElements> AllInterfaceXElements => InterfaceXElements;

        public static IEnumerable<IObjectXElement> AllObjectXElements => ObjectXElements;

        public static IEnumerable<IGroupXElement> AllObjectGroupXElements => GroupXElements;
    }
}