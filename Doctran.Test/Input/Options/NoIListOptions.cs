﻿// <copyright file="NoIListOptions.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Test.Input.Options
{
    using System.Collections.Generic;
    using Doctran.Input.Options;

    internal class NoIListOptions
    {
        [OptionList("NoIList", typeof(Dictionary<int, string>))]
        public IEnumerable<string> NoIList { get; set; }
    }
}