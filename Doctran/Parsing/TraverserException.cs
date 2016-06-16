// <copyright file="TraverserException.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing
{
    using System;

    public class TraverserException
        : ApplicationException
    {
        public TraverserException(object cause, string message)
            : base(message)
        {
            this.Cause = cause;
        }

        public TraverserException(object cause, string message, Exception innerException)
            : base(message, innerException)
        {
            this.Cause = cause;
        }

        public object Cause { get; }
    }
}