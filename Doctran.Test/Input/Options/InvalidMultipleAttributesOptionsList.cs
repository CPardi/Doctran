namespace Doctran.Test.Input.Options
{
    using System.Collections.Generic;
    using Doctran.Input.Options;

    internal class InvalidMultipleAttributesOptionsList
    {
        [OptionList("IntOption", typeof(List<int>))]
        [OptionList("StringOption", typeof(List<string>))]
        public List<string> OptionList { get; set; }
    }
}