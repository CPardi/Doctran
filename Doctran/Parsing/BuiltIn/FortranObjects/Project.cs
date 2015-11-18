//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Doctran.Parsing.BuiltIn.FortranObjects
{
    //public class Project : XFortranObject
    //{
    //    private DateTime _creationDate = DateTime.Now;
    //    private readonly XElement _xmlPassthrough;

    //    public Project(Options options, IEnumerable<FortranBlock> blockParsers)
    //    {
    //        var sourceParser = new Parser(blockParsers);

    //        _xmlPassthrough = options.XmlInformation;
    //        this.FilePath = options.ProjectFilePath;

    //        //// Parse source files.
    //        //try
    //        //{
    //        //    var files = (from path in options.SourceFilePaths.AsParallel()
    //        //        select sourceParser.ParseFile(path, File.ReadFile(path))).ToList();
    //        //    this.AddSubObjects(files);
    //        //}
    //        //catch (IOException e)
    //        //{
    //        //    Report.Error((pub, ex) =>
    //        //    {
    //        //        pub.AddErrorDescription("Error in specified source file.");
    //        //        pub.AddReason(e.Message);
    //        //    }, e);
    //        //}
    //    }

    //    public string FilePath { get; private set; }

    //    public override XElement XEle()
    //    {
    //        var xele = new XElement("Project");

    //        xele.Add(new XElement("Name", this.Name));
    //        xele.Add(_xmlPassthrough);
    //        xele.Add(new XElement("DocCreated", this._creationDate.ToXElement()));
    //        xele.Add(
    //            from info in this.SubObjectsOfType<Description2>()
    //            select info.XEle()
    //            );
    //        xele.Add(new XElement("Files",
    //            from file in this.SubObjectsOfType<SourceFile>().AsParallel()
    //            select file.XEle())
    //            );
    //        return xele;
    //    }

    //    protected override string GetIdentifier()
    //    {
    //        return this.Name;
    //    }
    //}
}