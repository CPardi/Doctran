//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

using Doctran.BaseClasses;

namespace Doctran.Fbase.Common
{
    public static class FlagParser
    {
        public static Settings Parse(String[] args)
        {
            var settings = new Settings();
            var flags = new List<Flag>
            {
                new Plugins(),
                new Save_xml(),
                new Output_dir(),
                new Project_info(),
                new Theme(),
                new Overwrite(),
                new Help(),
                new Extensions()
            };
            // The help flag requires the previous flags in order to output their help text.
            (flags.Single(f => f is Help) as Help).Flags = flags;

            // If there are no arguments write the help.
            if (args.Length == 0) { (flags.Single(f => f is Help) as Help).WriteHelp(); return null; }

            // Loop through each argument and look for match against the flags defined above.
            int i = 0;
            while (i < args.Length)
            {
                bool foundFlag = false;

                // Does the argument specify a source file.
                if (Regex.IsMatch(args[i], settings.FileWildcardRegex))
                {
                    foundFlag = true;

                    // In case wildcard is passed explicitly by enviromental, split off the filename and treat as a search pattern.
                    // This argument is a file path.
                    String argPath = args[i];

                    // The search pattern will be the filename.
                    string pattern = Path.GetFileName(argPath);

                    // Get the container directory.
                    string fileDir = argPath.Substring(0, argPath.Length - pattern.Length);

                    // Get absolute path.
                    try
                    {
                        string absPath = fileDir == "" ? "." + Settings.slash : Path.GetFullPath(fileDir);

                        string[] files = Directory.GetFiles(absPath, pattern.Trim(), SearchOption.TopDirectoryOnly);

                        // Add the files to the source files list.
                        settings.SourceFiles.AddRange(files);
                    }
                    catch (ArgumentException e)
                    {
                        UserInformer.GiveError("argument", args[i], e);
                    }
                    catch (IOException e)
                    {
                        UserInformer.GiveWarning("argument", e);
                    }
                    
                }
                else if (Regex.IsMatch(args[i], settings.FilePathRegex))
                {
                    foundFlag = true;

                    // If no wildcard specified just get the file's path.
                    String file = Path.GetFullPath(args[i]);

                    // Add the files to the source files list.
                    settings.SourceFiles.Add(file); 
                }

                // Or, is the current arguement a flag?
                foreach (var flag in flags)
                {
                    foundFlag |= flag.Check(args, ref i, settings);
                    if (foundFlag)
                        break;
                    if (settings == null) return null;
                }

                if (!foundFlag)
                {
                    Console.WriteLine("The flag " + args[i] + " is invalid.");
                    return null;
                }

                i++;
            }
                
            // If no source files are specified, then ask for some.
            if (settings.SourceFiles.Count == 0) { Console.WriteLine("Please specify one or more input files."); return null; }

            // Get any file specified twice.
            var distinct = settings.SourceFiles.Distinct().ToList();

            // If there is any duplicates tell the user.
            var duplicates = settings.SourceFiles.GroupBy(x => x).Where(group => group.Count() > 1).Select(group => group.Key);
            foreach (var duplFile in duplicates)
            {
                Console.WriteLine();
                Console.WriteLine("----------------------Warning----------------------");
                Console.WriteLine("The file '" + duplFile + "' has been specified more than once and was ignored.");
            }
            
            // Consider each file only once.
            settings.SourceFiles.Clear();
            settings.SourceFiles.AddRange(distinct);

            return settings;
        }
    }

	abstract class Flag
	{
        private readonly bool useShort = false;
		private readonly String shortFlag;

        private readonly bool useParam = false;
        protected readonly String flagParam;

        public abstract String Category { get; }

        protected Flag() { }

        protected Flag(String shortFlag = "", String flagParam = "")
		{
            if (shortFlag != "")
            {
                this.useShort = true;
                this.shortFlag = shortFlag;
            }

            if (flagParam != "")
            {
                this.useParam = true;
                this.flagParam = flagParam;
            }
		}

        public abstract String LongFlag { get; }
        public abstract String Description { get; }

        public bool Check(string[] args, ref int line_no, Settings settings)
        {
            bool thisFlag;
            if (this.useShort)
            {
                thisFlag = Regex.IsMatch(args[line_no], "\\s*^-" + this.shortFlag + "\\s*$|\\s*^--" + this.LongFlag + "\\s*$");
                if (thisFlag)
                    this.Action(args, ref line_no, settings);
            }
            else
            {
                thisFlag = Regex.IsMatch(args[line_no], "\\s*^--" + this.LongFlag + "\\s*$");
                if (thisFlag)
                    this.Action(args, ref line_no, settings);
            }
            return thisFlag;
        }

        protected abstract void Action(string[] args, ref int line_no, Settings settings);

		public void WriteDescription()
		{
            const int width = 27;
            if (this.useShort && this.useParam)
                Console.WriteLine(("    --" + this.LongFlag + ", -" + this.shortFlag + " " + this.flagParam + "    ").PadRight(width) + this.Description);
            else if (this.useShort && !this.useParam)
                Console.WriteLine(("    --" + this.LongFlag + ", -" + this.shortFlag + "    ").PadRight(width) + this.Description);
            else if (!this.useShort && this.useParam)
                Console.WriteLine(("    --" + this.LongFlag + " " + this.flagParam + "    ").PadRight(width) + this.Description);
            else
                Console.WriteLine(("    --" + this.LongFlag + "    ").PadRight(width) + this.Description);
		}
	}

    // Help

    class Plugins : Flag
    {
        protected override void Action(String[] args, ref int line_no, Settings settings)
        {            
            foreach(var plugin in PluginManager.Plugins)
            {
                plugin.WriteInformation();
                Console.WriteLine();
            }
            settings = null;
        }

        public override string Category{ get { return "Help"; } }
        public override string LongFlag { get { return "plugins"; } }
        public override string Description { get { return "This option forces Doctran to check the plugins folder with its installation path for any shared libraries present and write their names and version. Doctran will exit immediately after plugins have been checked."; } }
    }

    class Help : Flag
    {
        private List<Flag> flags;

        public Help() : base("h") { }

        public List<Flag> Flags
        {
            set
            {
                this.flags = value;
            }
        }

        public void WriteHelp()
        {
            Console.WriteLine("Usage is: Doctran [options] source_files");
            Console.WriteLine();
            Console.WriteLine("-- Options List --");
            foreach (var catagory in this.flags.GroupBy(f => f.Category))
            {
                
                Console.WriteLine(catagory.Key + ":");
                foreach (var flag in catagory)
                    flag.WriteDescription();
                Console.WriteLine();
            }
        }

        protected override void Action(String[] args, ref int line_no, Settings settings)
        {
            this.WriteHelp();
            settings = null;
        }

        public override string Category { get { return "Help"; } }
        public override string LongFlag
        {
            get
            {
                return "help";
            }
        }

        public override string Description
        {
            get
            {
                return "Displays usage information.";
            }
        }
    }

    // I/O

    class Save_xml : Flag
    {
        public Save_xml() : base(flagParam: "FILE") { }

        protected override void Action(String[] args, ref int line_no, Settings settings)
        {
            settings.save_xml = true; 
            settings.save_xml_name = args[line_no + 1]; 
            line_no++;
        }

        public override string Category { get { return "I/O"; } }
        public override string LongFlag { get { return "save_xml"; } }
        public override string Description { get { return "Tells Doctran to save the intermediary xml generated during the documentation's generation. " + this.flagParam + " is the name and/or path for the xml file."; } }
        
    }

    class Use_xml : Flag
    {
        public Use_xml() : base(flagParam: "FILE") { }

        protected override void Action(String[] args, ref int line_no, Settings settings)
        {
            settings.use_existing_xml = true; 
            settings.existing_xml_name = args[line_no + 1]; 
            line_no++;
        }

        public override string Category { get { return "I/O"; } }
        public override string LongFlag
        {
            get
            {
                return "use_xml";
            }
        }

        public override string Description
        {
            get
            {
                return "Tells Doctran to load an existing xml file rather than generating from code files. " + this.flagParam + " is the name or path specifying the xml file to load from.";
            }
        }
    }

    class Output_dir : Flag
    {
        public Output_dir() : base("o","DIR") { }

        protected override void Action(String[] args, ref int line_no, Settings settings)
        {
            settings.OutputDirectory = args[line_no + 1]; 
            line_no++;
        }

        public override string Category { get { return "I/O"; } }
        public override string LongFlag
        {
            get
            {
                return "output_dir";
            }
        }

        public override string Description
        {
            get
            {
                return this.flagParam + " is the output directory for the documentation.  (Default: Current Directory)";
            }
        }
    }

    // Project

    class Project_info : Flag
    {
        public Project_info() : base(flagParam: "FILE") { }

        protected override void Action(String[] args, ref int line_no, Settings settings)
        {
            settings.has_info = true;
            settings.ProjectInfo = args[line_no + 1]; 
            line_no++;
        }

        public override string Category { get { return "Project"; } }
        public override string LongFlag { get { return "project_info"; } }
        public override string Description { get { return this.flagParam + " is the name and/or path of the project's information file."; } }
    }

    // Themes

    class Theme : Flag
    {
        public Theme() : base("t", "NAME") { }

        protected override void Action(String[] args, ref int line_no, Settings settings)
        {
            settings.ThemeName = args[line_no + 1]; 
            line_no++;
        }

        public override string Category { get { return "Themes"; } }
        public override string LongFlag { get { return "theme"; } }
        public override string Description { get { return this.flagParam + " is the name of the theme to be applied. (Default: Default)"; } }        
    }

    class Overwrite : Flag
    {
        public Overwrite() : base(flagParam: "true/false") { }

        protected override void Action(String[] args, ref int line_no, Settings settings)
        {
            settings.overwriteTheme = args[line_no + 1].Equals("true"); 
            line_no++;
        }

        public override string Category { get { return "Themes"; } }
        public override string LongFlag { get { return "overwrite"; } }
        public override string Description { get { return @"If true then the auxillary files (css, scripts, etc.) and html files in the output directory will be overwritten. If false then just the html files will be overwritten. (Default: false)"; } }
    }

    class Extensions : Flag
    {
        public Extensions() : base(flagParam: "EXT") { }

        protected override void Action(String[] args, ref int line_no, Settings settings)
        {
            settings.Extensions.AddRange(args[line_no + 1].Split(','));
            line_no++;
        }

        public override string Category { get { return "I/O"; } }
        public override string LongFlag { get { return "extensions"; } }
        public override string Description { get { return @"EXT should be a comma separated list of any additional extensions your source files may have. (Default: f90,f95,f03,f08)"; } }
    }
}