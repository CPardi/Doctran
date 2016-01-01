// <copyright file="InvalidPropertyTypeException.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Input.Options
{
    using System;

    /// <summary>
    ///     This exception should be used to signal to the developer that a property is of an incorrect type to be used with
    ///     the attribute that was applied.
    /// </summary>
    public class InvalidPropertyTypeException : ApplicationException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="InvalidPropertyTypeException" /> class.
        /// </summary>
        /// <param name="attributeName">The name of the attribute applied that was applied.</param>
        /// <param name="expected">The property type that the attribute is valid for.</param>
        /// <param name="actual">The property type that the attribute was actually applied to.</param>
        public InvalidPropertyTypeException(string attributeName, Type expected, Type actual)
            : base($"Error in declaration of 'TOptions'. '{attributeName}' has been applied to a property of type " +
                   $"{actual.Name} and must only be applied to properties of type {expected.Name}.")
        {
        }
    }
}