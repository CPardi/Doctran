namespace Doctran.Test.Input.OptionsReader
{
    using System.Collections.Generic;
    using Doctran.Input.Options;

    internal class MultiListOptionsWithDefault
    {
        [OptionList("MultiOptionList1", typeof(List<int>))]
        [OptionList("MultiOptionList2", typeof(List<int>))]
        [DefaultOption(typeof(List<int>))]
        public IList<int> MultiOptionListWithDefault { get; set; }
    }
}