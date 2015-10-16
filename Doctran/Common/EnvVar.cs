//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.IO;

namespace Doctran.Fbase.Common
{
    using System.Reflection;

    public class EnvVar
    {
        public readonly static Char slash = Path.DirectorySeparatorChar;

        public static string execPath => Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath) + slash;

        public readonly static string defaultInfoPath = EnvVar.execPath + "defaults" + EnvVar.slash + "project.info";

        public static string ThemeDirectory(string themeName)
        {
            return execPath + @"themes" + slash + themeName + slash;
        }

        public static string XsltFullPathAndName(string themeName)
        {
            return ThemeDirectory(themeName) + "main";
        }

        public static int Verbose { get; set; }
    }
}