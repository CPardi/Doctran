//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Doctran.Utilitys
{
    using System;
    using System.IO;
    using System.Reflection;

    public class EnvVar
    {
        public static string PluginPath => Path.Combine(ExecPath, "plugins");

        public readonly static char Slash = Path.DirectorySeparatorChar;

        public static string ExecPath => Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath) + Slash;

        public static readonly string DefaultInfoPath = Path.Combine(EnvVar.ExecPath, "defaults", "project.info");

        public static string ThemeDirectory(string themeName) => Path.Combine(ExecPath, @"themes", themeName);

        public static string XsltFullPathAndName(string themeName) => Path.Combine(ThemeDirectory(themeName), "main");

        public static int Verbose { get; set; }
    }
}