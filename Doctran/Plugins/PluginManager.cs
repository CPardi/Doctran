// <copyright file="PluginManager.cs" company="Christopher Pardi">
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
    using System.Linq;
    using System.Text;
    using Helper;
    using Utilitys;

    public static class PluginManager
    {
        public static AssemblyLoader AssemblyLoader { get; } = new AssemblyLoader(Path.Combine(EnvVar.ExecPath, @"plugins"));

        public static string InformationString
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append("List of installed plugins.");
                sb.Append(Environment.NewLine);
                foreach (var p in Plugins)
                {
                    sb.Append(p.InformationString);
                    sb.Append(Environment.NewLine);
                }

                return sb.ToString();
            }
        }

        public static List<IPlugin> Plugins { get; private set; }

        public static void Initialize()
        {
            // Initialize plugins.
            PluginManager.Plugins = AssemblyLoader.GetClassInstances<IPlugin>();
            foreach (var plugin in Plugins.OrderBy(p => p.LoadOrder()))
            {
                plugin.Initialize();
            }
        }
    }
}