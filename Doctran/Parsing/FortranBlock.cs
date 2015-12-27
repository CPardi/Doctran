﻿// <copyright file="FortranBlock.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing
{
    using System.Collections.Generic;
    using Helper;

    public interface FortranBlock
    {
        bool CheckInternal { get; }

        bool ExplicitEnd { get; }

        string Name { get; }

        bool BlockEnd(string parentBlockName, List<FileLine> lines, int lineIndex);

        bool BlockStart(string parentBlockName, List<FileLine> lines, int lineIndex);

        /// <summary>
        ///     Returns one or more <see cref="FortranObject" />, that represent the block specified by <paramref name="lines" />.
        /// </summary>
        /// <param name="subObjects">The enumeration of <see cref="FortranObject" /> that are defined with the current block.</param>
        /// <param name="lines">Lines defining the object's creation.</param>
        /// <returns>An enumeration of <see cref="FortranObject" />.</returns>
        /// <exception cref="BlockParserException">This exception is thrown invalid content within <paramref name="lines" />.</exception>
        IEnumerable<FortranObject> ReturnObject(IEnumerable<IFortranObject> subObjects, List<FileLine> lines);
    }
}