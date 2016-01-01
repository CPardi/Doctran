// <copyright file="InvalidAttributesException.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Input.Options
{
    using System;

    /// <summary>
    ///     This exception should be used to signal to the developer that attributes have been used incorrectly.
    /// </summary>
    public class InvalidAttributesException : ApplicationException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="InvalidAttributesException" /> class.
        /// </summary>
        /// <param name="message">A message to describe the exception.</param>
        public InvalidAttributesException(string message)
            : base(message)
        {
        }
    }
}