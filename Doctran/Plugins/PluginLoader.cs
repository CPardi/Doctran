namespace Doctran.Plugins
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Helper;
    using Parsing;
    using Reporting;
    using Utilitys;

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
}