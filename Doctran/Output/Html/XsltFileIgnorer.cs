namespace Doctran.Output.Html
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using Assets;

    internal class XsltFileIgnorer : FileIgnorer
    {
        public override ReadOnlyCollection<string> FromExtensions => new[] { ".xslt", ".XSLT" }.ToList().AsReadOnly();

        public override string ToExtension => ".xslt";
    }
}