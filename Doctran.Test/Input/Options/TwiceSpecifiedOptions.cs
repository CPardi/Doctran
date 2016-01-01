// <copyright file="TwiceSpecifiedOptions.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Test.Input.Options
{
    using Doctran.Input.Options;

    internal class TwiceSpecifiedOptions
    {
        [Option("Option")]
        public int Option1 { get; set; }

        [Option("Option")]
        public int Option2 { get; set; }
    }
}