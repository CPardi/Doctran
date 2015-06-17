//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;

using Doctran.Fbase.Common;

namespace Doctran.BaseClasses
{
	public static class PluginManager
	{
		public static void Initialize()
		{
			// Load base objects.
			PluginManager.FortranBlocks = PluginManager.PluginLoader.GetClassInstances<FortranBlock>("Doctran.Fbase");
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

		private static readonly AssemblyLoader _pluginLoader = new AssemblyLoader(Settings.execPath + @"plugins");

		public static List<FortranBlock> FortranBlocks { get; set; }

		public static Dictionary<String, KeyValuePair<int, Traverser>> Traversers { get; set; }
		
		public static List<ObjectGroup> ObjectGroups { get; set; }

		public static AssemblyLoader PluginLoader
		{
			get
			{
				return _pluginLoader;
			}
		}
	}
}

