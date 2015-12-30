namespace Doctran.Test.Input.OptionsReader
{
    using Doctran.Input.Options;

    internal class TwiceSpecifiedOptions
    {
        [Option("Option")]
        public int Option1 { get; set; }

        [Option("Option")]
        public int Option2 { get; set; }
    }
}