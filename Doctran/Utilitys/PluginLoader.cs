namespace Doctran.Utility
{
    using System;
    using System.Collections.Generic;
    using Helper;
    using Utilitys;

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

        public static ILanguage GetLanguage(string extension)
        {
            ILanguage language;
            if (!Languages.TryGetValue(extension, out language))
            {
                throw new ArgumentException($"No language corresponds to the file extension '{extension}'.");
            }
            
            return language;
        }
    }
}