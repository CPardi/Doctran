namespace Doctran
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Linq;
    using Helper;
    using Output.Html;
    using Reporting;
    using Utilitys;

    public partial class Program
    {
        private static void OutputHtml(XmlOutputter xmlOutputter, XDocument sourceDocument, Options options)
        {
            StageStopwatch.Restart();
            Report.NewStatus("Generating htmls... ");

            var htmlOutputter = new XsltRunner(Path.Combine(EnvVar.ExecPath, "themes", options.ThemeName, "main.xslt"));

            htmlOutputter.Run(
                xmlOutputter.XDocument,
                Path.GetFullPath(options.OutputDirectory) + EnvVar.Slash,
                new KeyValuePair<string, object>("verbose", Report.Verbose),
                new KeyValuePair<string, object>("source", sourceDocument.CreateReader()));

            SaveTiming("html-output", StageStopwatch.ElapsedMilliseconds);
            Report.ContinueStatus("Done");
        }
    }
}