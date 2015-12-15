// <copyright file="FileLine.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Helper
{
    public struct FileLine
    {
        public FileLine(int number, string text)
            : this()
        {
            this.Number = number;
            this.Text = text;
        }

        public int Number { get; private set; }

        public string Text { get; private set; }
    }
}