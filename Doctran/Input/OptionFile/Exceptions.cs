// <copyright file="Exceptions.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Input.OptionFile
{
    using System;
    using Helper;

    public class WrongDepthException : Exception
    {
        public WrongDepthException(FileLine line, int expectedDepth, int actualDepth)
            : base("There is an error on line " + line.Number + ". The line '" + line.Text + "' is at level " + actualDepth + ", but was expected to be at level " + expectedDepth + ".")
        {
        }
    }
}