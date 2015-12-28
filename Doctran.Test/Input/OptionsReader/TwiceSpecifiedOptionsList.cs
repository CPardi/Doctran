namespace Doctran.Test.Input.OptionsReader
{
    using System.Collections.Generic;
    using Doctran.Input.OptionsReaderCore;

    internal class TwiceSpecifiedOptionsList
    {
        [OptionList("OptionList", typeof(List<int>))]
        public IList<int> OptionList1 { get; set; }

        [OptionList("OptionList", typeof(List<int>))]
        public IList<int> OptionList2 { get; set; }
    }
}