namespace Doctran.Output.Assets
{
    using System.IO;

    internal class DefaultCopier
    {
        public void Run(string filePath, string outputPath, bool overwrite)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(outputPath)));
            if (!File.Exists(outputPath) || overwrite)
            {
                File.Copy(filePath, outputPath, overwrite);
            }
        }
    }
}