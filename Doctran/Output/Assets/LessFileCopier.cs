namespace Doctran.Output.Assets
{
    using System.Collections.ObjectModel;
    using dotless.Core;
    using Utilitys;

    internal class LessFileCopier : TextFileCopier
    {        
        public override ReadOnlyCollection<string> FromExtensions => new[] { ".less", ".LESS" }.ToReadOnlyCollection();

        public override string ToExtension => ".css";

        protected override string ReadFile(string filePath)
        {
            return Less.Parse(base.ReadFile(filePath));
        }
    }
}