namespace Doctran.Test.Input.OptionsReader
{
    using Doctran.Input.OptionsReaderCore;

    internal class TwiceSpecifiedOptions
    {
        [Option("Option")]
        public int Option1 { get; set; }

        [Option("Option")]
        public int Option2 { get; set; }
    }
}