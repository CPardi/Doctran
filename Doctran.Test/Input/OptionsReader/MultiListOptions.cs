namespace Doctran.Test.Input.OptionsReader
{
    using System.Collections.Generic;
    using Doctran.Input.OptionsReaderCore;

    internal class MultiListOptions
    {
        [OptionList("MultiOptionList1", typeof(List<int>))]
        [OptionList("MultiOptionList2", typeof(List<int>))]
        public IList<int> MultiOptionList { get; set; }
    }
}