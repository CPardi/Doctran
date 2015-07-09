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
		private static readonly List<Flag> _flags
			= new List<Flag>();

		static FlagParser()
		{
			_flags.AddRange(new List<Flag>() { 
				new Plugins(),
				new SaveXml(),
				new OutputDir(),
				new ProjectInfo(),
				new Theme(),
				new ColorScheme(),
				new Overwrite(),
				new Verbose(),
				new Help(),
				new Extensions()
			});
		}

		public static void AddFlag(Flag flag)
		{
			_flags.Add(flag);
		}

		public static Settings Parse(String[] args)
		{
			var settings = new Settings();

			// The help flag requires the previous flags in order to output their help text.
			(_flags.Single(f => f is Help) as Help).Flags = _flags;

			// If there are no arguments write the help.
			if (args.Length == 0) { (_flags.Single(f => f is Help) as Help).WriteHelp(); return null; }

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
				foreach (var flag in _flags)
				{
					if (foundFlag) break;
					try
					{
						foundFlag |= flag.Check(args, ref i, settings);
					}
					catch(ArgumentException e){
						UserInformer.GiveError("argument", "the value for '" + args[i] + "' of '" + args[i+1] + "' is invalid", e);
					}
					if (foundFlag && flag.Terminate) Helper.Stop();
				}

				if (!foundFlag)
				{
					UserInformer.GiveError("argument list", args[i] + " is not a valid argument");
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

	public abstract class Flag
	{
		private readonly bool _useShort = false;
		private readonly String shortFlag;

		private readonly bool _useParam = false;
		protected readonly String flagParam;

		public abstract String Category { get; }

		protected Flag() { }

		protected Flag(String shortFlag = "", String flagParam = "", bool hasArgument = false, bool terminate = false)
		{
			if (shortFlag != "")
			{
				this._useShort = true;
				this.shortFlag = shortFlag;
			}

			if (flagParam != "")
			{
				this._useParam = true;
				this.flagParam = flagParam;
			}

			this.HasArgument = hasArgument;
			this.Terminate = terminate;
		}

		public bool HasArgument { get; private set; }
		public bool Terminate { get; private set; }
		public abstract String LongFlag { get; }
		public abstract String Description { get; }

		public bool Check(string[] args, ref int line_no, Settings settings)
		{
			bool thisFlag;
			if (this._useShort)
			{
				thisFlag = Regex.IsMatch(args[line_no], "\\s*^-" + this.shortFlag + "\\s*$|\\s*^--" + this.LongFlag + "\\s*$");
				if (thisFlag)
				{
					this.CheckInput(args, line_no);
					this.Action(args, ref line_no, settings);
				}
			}
			else
			{
				thisFlag = Regex.IsMatch(args[line_no], "\\s*^--" + this.LongFlag + "\\s*$");
				if (thisFlag)
				{
					this.CheckInput(args, line_no);
					this.Action(args, ref line_no, settings);
				}
			}
			return thisFlag;
		}

		protected void CheckInput(string[] args, int line_no)
		{
			try
			{
				if (this.HasArgument)
					if (args[line_no+1].StartsWith("--"))
					{
						UserInformer.GiveError("argument", "option '" + args[line_no] + "' requires a value to be specified.");
					}
			}
			catch(IndexOutOfRangeException) {
				UserInformer.GiveError("arguments", "Option '" + args[line_no] + "' requires a value to be specified.");
			}
		}

		protected abstract void Action(string[] args, ref int line_no, Settings settings);

		public void WriteDescription()
		{
			const int width = 27;
			if (this._useShort && this._useParam)
				Console.WriteLine(("    --" + this.LongFlag + ", -" + this.shortFlag + " " + this.flagParam + "    ").PadRight(width) + this.Description);
			else if (this._useShort && !this._useParam)
				Console.WriteLine(("    --" + this.LongFlag + ", -" + this.shortFlag + "    ").PadRight(width) + this.Description);
			else if (!this._useShort && this._useParam)
				Console.WriteLine(("    --" + this.LongFlag + " " + this.flagParam + "    ").PadRight(width) + this.Description);
			else
				Console.WriteLine(("    --" + this.LongFlag + "    ").PadRight(width) + this.Description);
		}
	}

	// Help

	class Plugins : Flag
	{
		public Plugins() : base(terminate: true) { }

		protected override void Action(String[] args, ref int line_no, Settings settings)
		{
			Console.WriteLine(Environment.NewLine + "Plugin List:");
			foreach(var plugin in PluginManager.Plugins)
			{
				plugin.WriteInformation();
				Console.WriteLine();
			}
		}

		public override string Category{ get { return "Help"; } }
		public override string LongFlag { get { return "plugins"; } }
		public override string Description { get { return "This option forces Doctran to check the plugins folder with its installation path for any shared libraries present and write their names and version. Doctran will exit immediately after plugins have been checked."; } }
	}

	class Verbose : Flag
	{
		public Verbose() :base(flagParam: "VALUE", hasArgument: true){ }

		protected override void Action(String[] args, ref int line_no, Settings settings)
		{
			try{ Settings.verbose = Convert.ToInt32(args[line_no + 1]); }
			catch(FormatException){
				UserInformer.GiveError("arguments", "Option '" + args[line_no] + "' requires an integer value");
			}
			line_no++;
		}

		public override string Category { get { return "Help"; } }
		public override string LongFlag
		{
			get
			{
				return "verbose";
			}
		}

		public override string Description
		{
			get
			{
				return "Sets how much information should written to the screen. VALUE is an integer between 1 and 3. (Default: 2)";
			}
		}
	}

	class Help : Flag
	{
		private List<Flag> flags;

		public Help() : base(shortFlag: "h", terminate: true) { }

		public List<Flag> Flags
		{
			set
			{
				this.flags = value;
			}
		}

		public void WriteHelp()
		{
			Console.WriteLine("Doctran Documentation Generator Version 1.2.1");
			Console.WriteLine("Usage is: doctran [options] source_files");
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

	class SaveXml : Flag
	{
		public SaveXml() : base(flagParam: "FILE", hasArgument: true) { }

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

	class OutputDir : Flag
	{
		public OutputDir() : base("o","DIR",true) { }

		protected override void Action(String[] args, ref int line_no, Settings settings)
		{
			try
			{
				settings.OutputDirectory = Path.GetFullPath(args[line_no + 1]);
				Directory.CreateDirectory(settings.OutputDirectory);
			}
			catch(ArgumentException)
			{
				settings = null;
				throw;
			}
			finally
			{
				line_no++;
			}
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

	class ProjectInfo : Flag
	{
		public ProjectInfo() : base(flagParam: "FILE", hasArgument: true) { }

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
		public Theme() : base("t", "NAME", true) { }

		protected override void Action(String[] args, ref int line_no, Settings settings)
		{
			settings.ThemeName = args[line_no + 1]; 
			line_no++;
		}

		public override string Category { get { return "Themes"; } }
		public override string LongFlag { get { return "theme"; } }
		public override string Description { get { return this.flagParam + " is the name of the theme to be applied. (Default: Default)"; } }        
	}

	class ColorScheme : Flag
	{
		public ColorScheme() : base("c", "NAME",true) { }

		protected override void Action(String[] args, ref int line_no, Settings settings)
		{
			settings.ColorScheme = args[line_no + 1];
			line_no++;
			
			if (!File.Exists(Settings.execPath + Settings.slash + "colorSchemes" + Settings.slash + settings.ColorScheme + ".less"))
			{
				throw new ApplicationException("Color scheme does not exist.");
			}
			
		}

		public override string Category { get { return "Themes"; } }
		public override string LongFlag { get { return "color_scheme"; } }
		public override string Description { get { return this.flagParam + " is the name of the color scheme to be used. (Default: Default)"; } }
	}

	class Overwrite : Flag
	{
		public Overwrite() : base(flagParam: "true/false", hasArgument: true) { }

		protected override void Action(String[] args, ref int line_no, Settings settings)
		{
			settings.overwriteTheme = args[line_no + 1].Equals("true", StringComparison.CurrentCultureIgnoreCase); 
			line_no++;
		}

		public override string Category { get { return "Themes"; } }
		public override string LongFlag { get { return "overwrite"; } }
		public override string Description { get { return @"If true then the auxillary files (css, scripts, etc.) and html files in the output directory will be overwritten. If false then just the html files will be overwritten. (Default: false)"; } }
	}

	class Extensions : Flag
	{
		public Extensions() : base(flagParam: "EXT", hasArgument: true) { }

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