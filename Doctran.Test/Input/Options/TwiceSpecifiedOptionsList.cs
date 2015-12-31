namespace Doctran.Test.Input.Options
{
    using System.Collections.Generic;
    using Doctran.Input.Options;

    internal class TwiceSpecifiedOptionsList
    {
        [OptionList("OptionList", typeof(List<int>))]
        public IList<int> OptionList1 { get; set; }

        [OptionList("OptionList", typeof(List<int>))]
        public IList<int> OptionList2 { get; set; }
    }
}