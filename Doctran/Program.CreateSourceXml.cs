namespace Doctran
{
    using System.Linq;
    using System.Xml.Linq;
    using ParsingElements.FortranObjects;
    using Plugins;

    public partial class Program
    {
        private static XDocument CreateSourceXml(Project project)
        {
            var xElements =
                from source in project.Sources
                let highlighter = DocumentationManager.TryGetDefinitionByIdentifier(source.Language)
                select new XElement(
                    "File",
                    new XElement("Identifier", source.Identifier),
                    highlighter.HighlightLines(source.OriginalLines));
            var sourceDocument = new XDocument(new XElement("Source", xElements));

            return sourceDocument;
        }
    }
}