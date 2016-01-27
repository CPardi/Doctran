// <copyright file="ParsingUtils.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Utilitys
{
    using System.Collections.Generic;
    using System.Linq;
    using Helper;

    public static class ParsingUtils
    {
        public static List<FileLine> TrimLines(List<FileLine> lines)
        {
            return lines
                .Select(line => new FileLine(line.Number, line.Text.Trim()))
                .ToList();
        }
    }
}