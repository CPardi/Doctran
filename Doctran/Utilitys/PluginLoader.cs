namespace Doctran.Utilitys
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Helper;
    using Parsing;
    using Reporting;

    public static class PluginLoader
    {
        public static void Initialize()
        {
            var assemblyLoader = new AssemblyLoader(EnvVar.PluginPath);
            var plugins = assemblyLoader.GetClassInstances<IPlugin>();

            foreach (var p in plugins)
            {
                p.Initialize();
            }
        }

        public static ILanguageParser GetLanguageFromExtension(string path)
        {
            var ext = Path.GetExtension(path);
            ILanguageParser languageParser;
            if (LanguageParserManager.TryGetLanguage(ext, out languageParser))
            {
                return languageParser;
            }

            var e = new ApplicationException($"'{path}' could not be parsed as no language is registered to the file extension '{ext}'.");
            Report.Error((pub, ex) => { pub.AddErrorDescription(ex.Message); }, e);
            throw e;
        }

        public static IEnumerable<IInterfaceXElements> AllInterfaceXElements => DocumentationElementManager.AllInterfaceXElements;

        public static IEnumerable<IObjectXElement> AllObjectXElements => DocumentationElementManager.AllObjectXElements;

        public static IEnumerable<IGroupXElement> AllObjectGroupXElements => DocumentationElementManager.AllObjectGroupXElements;
    }

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