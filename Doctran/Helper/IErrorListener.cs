// <copyright file="IErrorListener.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Helper
{
    using System;

    public interface IErrorListener<in TException>
        where TException : Exception
    {
        /// <summary>
        ///     Report an error to the listener.
        /// </summary>
        /// /// <param name="exception">Exception causing the error.</param>
        void Error(TException exception);

        /// <summary>
        /// Report an warning to the listener.
        /// </summary>
        /// <param name="exception">Exception causing the warning.</param>
        void Warning(TException exception);
    }
}