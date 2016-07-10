namespace Doctran
{
    using System.Xml.Linq;
    using Output.Html;
    using ParsingElements.FortranObjects;
    using Reporting;

    public partial class Program
    {
        private static XmlOutputter GetXmlOutputter(Project project, XElement xmlInformation)
        {
            StageStopwatch.Restart();
            Report.NewStatus("Generating xml... ");
            var xmlOutputter = new XmlOutputter(project.XEle(xmlInformation));

            SaveTiming("xml-generation", StageStopwatch.ElapsedMilliseconds);
            Report.ContinueStatus("Done");
            return xmlOutputter;
        }
    }
}