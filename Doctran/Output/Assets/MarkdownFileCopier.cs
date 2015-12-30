namespace Doctran.Output.Assets
{
    using System.Collections.ObjectModel;
    using MarkdownSharp;
    using Utilitys;

    internal class MarkdownFileCopier : TextFileCopier
    {
        private readonly Markdown _parser = new Markdown();

        public override ReadOnlyCollection<string> FromExtensions => new[] { ".md", ".MD", ".markdown", ".MARKDOWN" }.ToReadOnlyCollection();

        public override string ToExtension => ".html";

        protected override string ReadFile(string filePath)
        {
            return _parser.Transform(base.ReadFile(filePath));
        }
    }
}