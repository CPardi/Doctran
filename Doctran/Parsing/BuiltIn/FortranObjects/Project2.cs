namespace Doctran.Parsing.BuiltIn.FortranObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Plugins;
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
            xele.Add(new XElement("DocCreated", DateTime.Now.ToXElement()));
            xele.Add(
                from info in this.SubObjectsOfType<Description2>()
                select new XElement("Description", info.Basic, info.Detailed)
                );

            xele.Add(new XmlGenerator(PluginLoader.AllInterfaceXElements, PluginLoader.AllObjectXElements, PluginLoader.AllObjectGroupXElements).CreateForObject(this));

            return xele;
        }

        protected override string GetIdentifier() => this.Name;
    }
}