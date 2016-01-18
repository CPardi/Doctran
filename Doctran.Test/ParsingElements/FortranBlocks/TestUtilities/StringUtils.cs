// <copyright file="StringUtils.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Test.ParsingElements.FortranBlocks.TestUtilities
{
    using System.Collections.Generic;
    using System.Linq;
    using Doctran.Helper;

    public class StringUtils
    {
        public static List<FileLine> ConvertToFileLineList(string linesString)
        {
            var lines = new List<FileLine>();
            lines.AddRange(
                linesString
                    .Split('\n')
                    .Select((l, i) => new FileLine(i, l)));
            return lines;
        }
    }
}