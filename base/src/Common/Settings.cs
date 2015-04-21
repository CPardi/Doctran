//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Doctran.Fbase.Common
{
    public class Settings
    {
		public readonly static Char slash = Path.DirectorySeparatorChar;

		public readonly String execPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + slash;

		// Project settings.
        public bool has_info = false;
		public String ProjectInfo;
        public List<String> Extensions = new List<String>() { "f90", "f95", "f03", "f08" , "F90", "F95", "F03", "F08"};
        public String FilePathRegex{
            get{
                return @"\w+\." + this.Extensions.First() + String.Concat(
                    from ext in this.Extensions.Skip(1)
                    select @"|\w+\." + ext);
            }
        }

        public String FileWildcardRegex{
            get{
                return @"\*\." + this.Extensions.First() + String.Concat(
                    from ext in this.Extensions.Skip(1)
                    select @"|\*\." + ext);
            }
        }

        public List<String> SourceFiles = new List<String>();
		private String _OutputDirectory = "." + slash;
		public String OutputDirectory {
			get {
				return this._OutputDirectory;
			}
			set {
				if(value[value.Length-1] != slash) this._OutputDirectory = value + slash;
				else this._OutputDirectory = value;
			}
		}
		public String OutputFullPath {
			get {
				return Path.GetFullPath(OutputDirectory);
			}
		}

		// XML IO settings.
		public bool save_xml = false;
		public String save_xml_name = "";
		public bool use_existing_xml = false;
		public String existing_xml_name = "";

		// Theme settings.
		public String xsltName = @"main.xslt";
        public bool overwriteTheme = false;
		public String ThemeName = "Default";
		public String ThemesDirectory = @"themes" + slash;
		public String ThemeFullPath {
			get {
				return this.execPath + this.ThemesDirectory + this.ThemeName + slash;
			}
		}
		public String XsltFullPathAndName {
			get {
                return ThemeFullPath + xsltName;
			}
		}
    }
}