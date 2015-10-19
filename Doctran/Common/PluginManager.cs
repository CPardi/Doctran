//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.Linq;

using Doctran.Fbase.Common;
using Doctran.Fbase.Comments;

namespace Doctran.BaseClasses
{
    using System;
    using System.Text;

    public static class PluginManager
	{
        public static void Initialize()
        {
            // Load base objects.
            PluginManager.FortranBlocks = new List<FortranBlock>();
            PluginManager.FortranBlocks.Add(new NamedDescriptionBlock());
            PluginManager.FortranBlocks.Add(new DescriptionBlock());

            PluginManager.ObjectGroups = PluginManager.PluginLoader.GetClassInstances<ObjectGroup>("Doctran.Fbase");

            PluginManager.Traversers = new Dictionary<string, KeyValuePair<int, Traverser>>();
            PluginManager.Traversers.Add("Linker", new KeyValuePair<int, Traverser>(0, new Traverser(PluginManager.PluginLoader.GetClassInstances<PostAction>("Doctran.Fbase"))));
            PluginManager.Traversers.Add("ErrorChecker", new KeyValuePair<int, Traverser>(1, new Traverser(PluginManager.PluginLoader.GetClassInstances<PostAction>("Doctran.Exceptions"))));

            //Initialize plugins.
            PluginManager.Plugins = PluginManager.PluginLoader.GetClassInstances<IPlugin>();
            foreach (var plugin in Plugins.OrderBy(p => p.LoadOrder()))
                plugin.Initialize();
        }

		public static List<IPlugin> Plugins { get; private set; }

        public static List<FortranBlock> FortranBlocks { get; set; }

		public static Dictionary<string, KeyValuePair<int, Traverser>> Traversers { get; set; }
		
		public static List<ObjectGroup> ObjectGroups { get; set; }

		public static AssemblyLoader PluginLoader { get; } = new AssemblyLoader(EnvVar.execPath + @"plugins");

        public static string InformationString
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append("List of installed plugins.");
                sb.Append(Environment.NewLine);
                foreach (var p in PluginManager.Plugins)
                {
                    sb.Append(p.InformationString);
                    sb.Append(Environment.NewLine);
                }

                return sb.ToString();
            }
        }
	}
}

