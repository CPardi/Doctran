// <copyright file="PluginLoader.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

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
        public static IEnumerable<IInterfaceXElements> AllInterfaceXElements => DocumentationElementManager.AllInterfaceXElements;

        public static IEnumerable<IGroupXElement> AllObjectGroupXElements => DocumentationElementManager.AllObjectGroupXElements;

        public static IEnumerable<IObjectXElement> AllObjectXElements => DocumentationElementManager.AllObjectXElements;

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

        public static void Initialize()
        {
            var assemblyLoader = new AssemblyLoader(EnvVar.PluginPath);
            var plugins = assemblyLoader.GetClassInstances<IPlugin>();

            foreach (var p in plugins)
            {
                p.Initialize();
            }
        }
    }
}