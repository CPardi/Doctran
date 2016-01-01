// <copyright file="OptionReaderException.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Input.Options
{
    using System;

    public class OptionReaderException : ApplicationException
    {
        public OptionReaderException(int startLine, int endLine, string message)
            : base(message)
        {
            this.StartLine = startLine;
            this.EndLine = endLine;
        }

        public int EndLine { get; set; }

        public int StartLine { get; set; }
    }
}