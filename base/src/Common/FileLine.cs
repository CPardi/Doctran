//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;

namespace Doctran.Fbase.Common
{
    public struct FileLine
    {
        public int Number { get; private set; }
        public String Text { get; private set; }

        public FileLine(int number, String text) :this()
        {
            this.Number = number;
            this.Text = text;
        }
    }
}