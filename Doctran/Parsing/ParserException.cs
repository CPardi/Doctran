// <copyright file="ParserException.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing
{
    using System;

    public class ParserException : ApplicationException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ParserException" /> class.
        /// </summary>
        /// <param name="startLine">The number of the first line on which the incorrect text was found.</param>
        /// <param name="endLine">The number of the last line on which the incorrect text was found.</param>
        /// <param name="message">A description of the exception that occured.</param>
        public ParserException(int startLine, int endLine, string message)
            : base(message)
        {
            this.StartLine = startLine;
            this.EndLine = endLine;
        }

        /// <summary>
        ///     Gets the number of the last line on which the incorrect text was found.
        /// </summary>
        public int EndLine { get; }

        /// <summary>
        ///     Gets the number of the first line on which the incorrect text was found.
        /// </summary>
        public int StartLine { get; }
    }
}