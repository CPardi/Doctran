namespace Doctran.Output
{
    using System.Collections.ObjectModel;

    internal interface IFileCopier
    {
        ReadOnlyCollection<string> FromExtensions { get; }
        
        string ToExtension { get; }

        void Run(string inputPath, string outputPath, bool overwrite);
    }
}