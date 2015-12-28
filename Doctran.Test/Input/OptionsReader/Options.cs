//-----------------------------------------------------------------------
// <copyright file="Options.cs" company="Christopher Pardi">
// Copyright © 2015 Christopher Pardi
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>
//-----------------------------------------------------------------------

namespace Doctran.Test.Input.OptionsReader
{
    using System.Collections.Generic;
    using Doctran.Input.OptionsReaderCore;

    internal class Options
    {
        [DefaultOption(typeof(List<string>))]
        public IList<string> DefaultOption { get; set; }

        [Option("ScalarInt", DefaultValue = 1)]
        public int IntOption { get; set; }

        [Option("ScalarString")]
        public string StringOption { get; set; }

        [OptionList("IntList", typeof(List<int>))]
        public IList<int> IntListOption { get; set; }

        [OptionList("StringEnumerable", typeof(List<string>), InitializeAsDefault = true)]
        public IEnumerable<string> StringEnumerableOption { get; set; }

        [OptionList("StringList", typeof(List<string>))]
        public IList<string> StringListOption { get; set; }
    }
}