namespace Doctran.Output.Assets
{
    using System.Collections.ObjectModel;

    public abstract class FileIgnorer : IFileCopier
    {
        public abstract ReadOnlyCollection<string> FromExtensions { get; }

        public abstract string ToExtension { get; }

        public void Run(string filePath, string outputPath, bool overwrite)
        {
        }
    }
}