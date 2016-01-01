// <copyright file="StandardErrorListener.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Helper
{
    using System;

    public class StandardErrorListener<TException> : IErrorListener<TException>
        where TException : Exception
    {
        public void Error(TException exception)
        {
            throw exception;
        }

        public void Warning(TException exception)
        {
            throw exception;
        }
    }
}