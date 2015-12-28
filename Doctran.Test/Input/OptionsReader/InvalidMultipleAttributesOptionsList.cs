namespace Doctran.Test.Input.OptionsReader
{
    using System.Collections.Generic;
    using Doctran.Input.OptionsReaderCore;

    internal class InvalidMultipleAttributesOptionsList
    {
        [OptionList("IntOption", typeof(List<int>))]
        [OptionList("StringOption", typeof(List<string>))]
        public List<string> OptionList { get; set; }
    }
}