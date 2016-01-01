// <copyright file="BlockParserException.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing
{
    using System;

    [Serializable]
    public class BlockParserException : ApplicationException
    {
        public BlockParserException(string message)
            : base(message)
        {
        }
    }
}