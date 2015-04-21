//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.IO;
using System.Linq;

using Doctran.Fbase.Projects;
using Doctran.Fbase.Common;
using Doctran.Fbase.Outputters;

using Doctran.BaseClasses;

namespace Doctran.ConsoleRunner
{
	class MainClass
	{
		static int Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

            PluginManager.Initialize();

			Settings settings = FlagParser.Parse(args);
			if (settings == null) return 1;

			// Setup project and output HTMLs.
			XmlOutputter xmlOutputter;
			if (settings.use_existing_xml)
			{
				xmlOutputter = new XmlOutputter(settings.OutputDirectory + Settings.slash + settings.existing_xml_name);
			}
			else
			{
				Console.Write ("Analysing project block structure... ");
				var project = new Project(settings);

                Console.Write("Done" + Environment.NewLine + "Post processing project... ");
                foreach (var traverser in PluginManager.Traversers.ToList().OrderBy(t => t.Value.Key))
                    traverser.Value.Value.Go(project);                    

                Console.Write("Done" + Environment.NewLine + "Generating xml... ");
				xmlOutputter = new XmlOutputter(project.XEle());
			}
			if (settings.save_xml) { xmlOutputter.SaveToDisk(settings.OutputDirectory + settings.save_xml_name); }

            Console.Write("Done" + Environment.NewLine + "Outputting theme files... ");
			var themeOutputter = new ThemeOutputter(settings.overwriteTheme);
			themeOutputter.Output(settings.ThemeFullPath, settings.OutputFullPath);

            Console.Write("Done" + Environment.NewLine + "Generating htmls... ");
			var htmlOutputter = new HtmlOutputter(settings.XsltFullPathAndName);
			htmlOutputter.SaveToDisk(xmlOutputter.XDocument, settings.OutputDirectory);

			Console.WriteLine("Done");
			Console.WriteLine(@"Documentation can be found at """ + settings.OutputFullPath + @"""");
			Console.WriteLine(@"Documentation generation complete.");
			return 0;
		}


		static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
		{
			try
			{
				Exception ex = (Exception)e.ExceptionObject;

				string logPath = Path.GetFullPath("errors.log");
				Console.WriteLine("internal error");
				Console.WriteLine("Exception details written to " + logPath);
				using (var sq = new StreamWriter (logPath, false)) {

                    sq.WriteLine("======Message======");
                    sq.WriteLine(ex.Message);
                    sq.WriteLine("");

                    sq.WriteLine("======StackTrace======");
                    sq.WriteLine(ex.StackTrace);
                    sq.WriteLine("");

                    sq.WriteLine("======Data======");
                    sq.WriteLine(ex.Data);
                    sq.WriteLine("");

                    sq.WriteLine("======InnerException======");
                    sq.WriteLine(ex.InnerException);
                    sq.WriteLine("");

                    sq.WriteLine("======Source======");
                    sq.WriteLine(ex.Source);
                    sq.WriteLine("");
				}
			}
			finally
			{
				Environment.Exit(1);
			}
		}
}
}