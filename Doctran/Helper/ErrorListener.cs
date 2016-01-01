// <copyright file="ErrorListener.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Helper
{
    using System;

    public class ErrorListener<TException> : IErrorListener<TException>
        where TException : Exception
    {
        private readonly Action<TException> _error;

        private readonly Action<TException> _warning;

        public ErrorListener(Action<TException> warning, Action<TException> error)
        {
            _warning = warning;
            _error = error;
        }

        public void Error(TException exception)
        {
            _error(exception);
        }

        public void Warning(TException exception)
        {
            _warning(exception);
        }
    }
}