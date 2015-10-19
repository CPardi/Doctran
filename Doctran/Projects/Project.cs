//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Doctran.Fbase.Common;
using Doctran.BaseClasses;
using Doctran.Fbase.Comments;

namespace Doctran.Fbase.Projects
{
    using System.IO;
    using Reporting;
    using File = Files.File;

    public class ProjectPostAction : PostAction
    {
        private List<File> _files;

        public ProjectPostAction() : base(typeof(Project)) { }

        public override void PostObject(ref FortranObject obj)
        {
            LoadFiles(obj);
            CheckFilenames(0);            
        }

        private void CheckFilenames(int depth)
        {
            List<string> sameNames = new List<string>();
            sameNames.AddRange((from file in _files
                                 where _files.Count(f => f.Name.ToLower() == file.Name.ToLower()) > 1
                                 select file.Name.ToLower()).Distinct());
            if (!sameNames.Any()) return;
            
            foreach (var name in sameNames)
            {
                var list = _files.Where(f => f.Name.ToLower() == name);
                foreach (var file in list)
                {
                    System.IO.DirectoryInfo path = System.IO.Directory.GetParent(file.PathAndFilename);
                    for (int d = 0; d < depth; d++) { path = path.Parent; }

                    file.Name = path.Name.ToLower() + EnvVar.slash + file.Name;

                    Console.WriteLine(file.Name);
                }
            }

            CheckFilenames(depth + 1);
        }

        private void LoadFiles(FortranObject obj)
        {
            if (_files == null) _files = obj.SubObjectsOfType<File>();
        }
    }

    public class Project : XFortranObject
    {        
        private DateTime _creationDate = DateTime.Now;        
        private Parser _sourceParser = new Parser(PluginManager.FortranBlocks);
        private XElement _xmlPassthrough;

        public string FilePath { get; private set; }
        
        public Project(Options options)
        {
            _xmlPassthrough = options.XmlInformation;
            this.FilePath = options.ProjectFilePath;

            // Add information blocks for this output.
            _sourceParser = new Parser(
                PluginManager.FortranBlocks.Concat(
                    from i in Enumerable.Range(1, 4)
                    select new InformationBlock(i))
                );

            // Parse source files.
            try
            {
                var files = (from path in options.SourceFilePaths.AsParallel()
                             select _sourceParser.ParseFile(path, File.ReadFile(path))).ToList();            
                this.AddSubObjects(files);
            }
            catch (IOException e) { Report.Error((pub, ex) =>
            {
                pub.AddErrorDescription("Error in specified source file.");
                pub.AddReason(e.Message);
            }, e); }

        }

        protected override string GetIdentifier()
        {
            return this.Name;
        }

        public override XElement XEle()
        {
            XElement xele = new XElement("Project");

            xele.Add(new XElement("Name", this.Name));
            xele.Add(_xmlPassthrough);
            xele.Add(new XElement("DocCreated", this._creationDate.XEle()));
            xele.Add(
                from info in this.SubObjectsOfType<Description>()
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
