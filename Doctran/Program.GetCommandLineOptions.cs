namespace Doctran
{
    using CommandLine;
    using Helper;
    using Plugins;
    using Reporting;

    public partial class Program
    {
        private static void GetCommandLineOptions(string[] args, Options options)
        {
            Parser.Default.ParseArgumentsStrict(args, options);

            ShowLicensing = options.ShowLicensing;

            PluginManager.Initialize(); // Must come after show licensing.

            if (options.ShowPluginInformation)
            {
                Report.MessageAndExit(PluginManager.InformationString);
            }
        }
    }
}