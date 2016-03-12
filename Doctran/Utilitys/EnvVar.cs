// <copyright file="EnvVar.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Utilitys
{
    using System;
    using System.IO;
    using System.Reflection;

    public class EnvVar
    {
        public static readonly char Slash = Path.DirectorySeparatorChar;

        public static readonly string DefaultInfoPath = Path.Combine(ExecPath, "defaults", "project.info");

        public static string ExecPath => Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath) + Slash;

        public static string PluginPath => Path.Combine(ExecPath, "plugins");

        public static string ThemeOutputPath(string themeName) => Path.Combine(ExecPath, @"themes", themeName);

        public static string ThemeOutputPath(string themeName, string fileName) => Path.Combine(ExecPath, @"themes", themeName, fileName);

        public static string XsltFullPathAndName(string themeName) => Path.Combine(ThemeOutputPath(themeName), "main");

        public static string XmlOutputPath(string outputDirectory) => Path.Combine(outputDirectory, "xml");

        public static string XmlOutputPath(string outputDirectory, string xmlName) => Path.Combine(outputDirectory, "xml", xmlName);
    }
}