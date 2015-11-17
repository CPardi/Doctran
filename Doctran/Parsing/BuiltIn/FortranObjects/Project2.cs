namespace Doctran.Parsing.FortranObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Utilitys;

    public class Project2 : FortranObject
    {
        public Project2(IEnumerable<SourceFile> parsedFiles)
            :base(parsedFiles, null)
        {
        }

        public XElement XEle(XElement xmlPassthrough)
        {
            var xele = new XElement("Project");

            xele.Add(new XElement("Name", this.Name));
            xele.Add(xmlPassthrough);
            xele.Add(new XElement("DocCreated", DateTime.Now.XEle()));
            xele.Add(
                from info in this.SubObjectsOfType<Description>()
                select info.XEle()
                );

            xele.Add(new XmlGenerator(PluginLoader.AllInterfaceXElements, PluginLoader.AllObjectXElements, PluginLoader.AllObjectGroupXElements).CreateForObject(this));

            return xele;
        }

        protected override string GetIdentifier() => this.Name;
    }
}