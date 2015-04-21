//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;

using Doctran;
using Doctran.Fbase.Common;
using Doctran.BaseClasses;

using Doctran.Fbase.Files;
using Doctran.Fbase.Comments;

namespace Doctran.Fbase.Projects
{
    public class Project : XFortranObject
    {
        public string OutputDirectory;
        DateTime CreationDate = DateTime.Now;

		public Project(Common.Settings settings)
        {
            if (settings.has_info && System.IO.File.Exists(settings.ProjectInfo))
            {
                this.lines =
                    (from line in File.ReadFile(System.IO.Path.GetFullPath(settings.ProjectInfo))
                     select new FileLine(line.Number, "!>" + line.Text)
                    ).ToList();
                this.Search();
            }

            this.OutputDirectory = settings.OutputDirectory;

			this.SubObjects.AddRange(
				from path in settings.SourceFiles.AsParallel()
                where System.IO.File.Exists(path)
				select new File(this, System.IO.Path.GetFullPath(path))
			);


            var unfoundFile = settings.SourceFiles.Where(path => !System.IO.Directory.Exists(path))
                                           .Where(path => !System.IO.File.Exists(path)).ToList();
            unfoundFile.ForEach(path =>
            {
                Console.WriteLine("----------------------Warning----------------------");
                Console.WriteLine("File '" + path + "' does not exist and was ignored.");
            }
            );
        }

        private static List<FileLine> Add(List<FileLine> lines)
        {
            return
                (from line in lines
                select new FileLine(line.Number, "!>" + line.Text)
                ).ToList();
        }

        protected override String GetIdentifier()
        {
            return this.Name;
        }

        public override XElement XEle()
        {
            XElement xele = new XElement("Project");
            if (this.SubObjectsOfType<Information>().All(info => info.Type != "Name")) { xele.Add(new XElement("Name","My Project")); }
            xele.Add(new XElement("DocCreated", this.CreationDate.XEle()));
            xele.Add(
                from info in this.SubObjectsOfType<Information>()
                select info.XEle()
                    );
            xele.Add(new XElement("Files",
				from file in this.SubObjectsOfType<File>().AsParallel()
				select file.XEle())
                    );
            return xele;
        }
    }
}
