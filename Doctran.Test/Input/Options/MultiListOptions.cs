namespace Doctran.Test.Input.Options
{
    using System.Collections.Generic;
    using Doctran.Input.Options;

    internal class MultiListOptions
    {
        [OptionList("MultiOptionList1", typeof(List<int>))]
        [OptionList("MultiOptionList2", typeof(List<int>))]
        public IList<int> MultiOptionList { get; set; }
    }
}