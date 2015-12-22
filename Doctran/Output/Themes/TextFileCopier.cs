namespace Doctran.Output
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using Utilitys;

    public abstract class FileIgnorer : IFileCopier
    {
        public abstract ReadOnlyCollection<string> FromExtensions { get; }

        public abstract string ToExtension { get; }

        public void Run(string filePath, string outputPath, bool overwrite)
        {
        }
    }

    internal abstract class TextFileCopier : IFileCopier
    {        
        public abstract ReadOnlyCollection<string> FromExtensions { get; }

        public abstract string ToExtension { get; }

        public void Run(string filePath, string outputPath, bool overwrite)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
            if (overwrite || !File.Exists(outputPath))
            {
                File.WriteAllText(outputPath, this.ReadFile(filePath));
            }
        }

        protected string ChangeExtension(string filePath)
        {
            var actualExtension = Path.GetExtension(filePath) ?? string.Empty;
            Debug.Assert(this.FromExtensions.Contains(actualExtension), $"The file path has the unexpected extension '{actualExtension}'.");
            return filePath.Substring(0, filePath.Length - actualExtension.Length) + this.ToExtension;
        }

        protected virtual string ReadFile(string filePath)
        {
            using (var fileReader = new StreamReader(filePath))
            {
                return fileReader.ReadToEnd();
            }
        }
    }
}