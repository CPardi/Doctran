namespace Doctran.Utilitys
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Helper;
    using Reporting;

    public static class PluginLoader
    {
        private static Dictionary<string, ILanguage> Languages { get; } = new Dictionary<string, ILanguage>();

        public static void Initialize()
        {
            var assemblyLoader = new AssemblyLoader(EnvVar.PluginPath);
            var plugins = assemblyLoader.GetClassInstances<IPlugin>();

            foreach (var p in plugins)
            {
                p.Initialize();
            }
        }

        public static void RegisterLanguage(string extension, ILanguage language)
        {
            Languages.Add(PathUtilitys.DottedExtension(extension), language);
        }

        public static ILanguage GetLanguageFromExtension(string path)
        {
            var ext = Path.GetExtension(path);
            ILanguage language;
            if (!LanguageManager.TryGetLanguage(ext, out language))
            {
                var e = new ApplicationException($"'{path}' could not be parsed as no language is registered to the file extension '{ext}'.");
                Report.Error((pub, ex) => { pub.AddErrorDescription(ex.Message); }, e);
                throw e;
            }
            return language;
        }

        
    }
}