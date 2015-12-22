namespace Doctran.Output
{
    using System.Collections.ObjectModel;
    using System.Linq;

    internal class XsltFileIgnorer : FileIgnorer
    {
        public override ReadOnlyCollection<string> FromExtensions => new[] { ".xslt", ".XSLT" }.ToList().AsReadOnly();

        public override string ToExtension => ".xslt";
    }
}