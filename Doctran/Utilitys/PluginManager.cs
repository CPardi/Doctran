//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Doctran.Utilitys
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Comments;
    using Helper;
    using Output;
    using Parsing;
    using Parsing.FortranBlocks;
    using Parsing.ObjectGroups;
    using Parsing.PostActions;

    public static class PluginManager
	{
        public static void Initialize()
        {
            // Load base objects.
            PluginManager.FortranBlocks = new List<FortranBlock>
            {
                new NamedDescriptionBlock(),
                new DescriptionBlock()
            };

            PluginManager.ObjectGroups = new List<ObjectGroup>()
            {
                new NamedDescriptionGroup(),
                new DescriptionGroup()
            };

            PluginManager.Traversers = new Dictionary<string, KeyValuePair<int, Traverser>>();
            PluginManager.Traversers.Add("Linker", new KeyValuePair<int, Traverser>(0,
                new Traverser(
                    new List<PostAction>()
                    {
                        new ProjectPostAction(),
                        new DescriptionPostAction(),
                    })));
            PluginManager.Traversers.Add("ErrorChecker", new KeyValuePair<int, Traverser>(1, new Traverser(
                new List<PostAction>()
                {
                    new DescriptionException()
                })));

            //Initialize plugins.
            PluginManager.Plugins = PluginManager.PluginLoader.GetClassInstances<IPlugin>();
            foreach (var plugin in Plugins.OrderBy(p => p.LoadOrder()))
                plugin.Initialize();
        }

        public static List<IPlugin> Plugins { get; private set; }

        public static List<FortranBlock> FortranBlocks { get; set; }

		public static Dictionary<string, KeyValuePair<int, Traverser>> Traversers { get; set; }
		
		public static List<ObjectGroup> ObjectGroups { get; set; }

		public static AssemblyLoader PluginLoader { get; } = new AssemblyLoader(EnvVar.ExecPath + @"plugins");

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

