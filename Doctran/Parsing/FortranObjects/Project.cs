//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Doctran.Parsing.FortranObjects
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using FortranBlocks;
    using Helper;
    using Parsing;
    using Reporting;
    using Utilitys;

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
                             select _sourceParser.ParseFile(path, FortranObjects.File.ReadFile(path))).ToList();            
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
                from file in this.SubObjectsOfType<FortranObjects.File>().AsParallel()
                select file.XEle())
                    );
            return xele;
        }
    }
}
