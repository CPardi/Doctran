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
    using System.Text.RegularExpressions;
    using System.Xml.Linq;
    using Helper;

    public static class ParsingUtils
    {
        /// <summary>
        ///     Array of macro names for <see cref="ReplaceMacros(string)" />.
        /// </summary>
        private static readonly string[] MacroNames = { "name", "blockname", "list", "table" };

        /// <summary>
        ///     Cached regex used within <see cref="ReplaceMacros(string)" />.
        /// </summary>
        private static readonly Regex MacroRegex = new Regex($@"\|\s*({MacroNames.DelimiteredConcat("|")})\s*(?:\s*\,\s*(\w+?))*\s*\|", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        ///     Replaces macros of the form |name, option1, ..., optionN| in a string with an XML representation of the form <macro type="name"><option1 />...<optionN/></macro>.
        /// </summary>
        /// <param name="source">The string to be processed.</param>
        /// <returns>The modified string with macros replaced.</returns>
        public static string ReplaceMacros(string source)
            => MacroRegex.Replace(
                source,
                m => new XElement(
                    "macro",
                    new XAttribute("name", m.Groups[1].Value.ToLower()),
                    m.Groups[2].Captures.Cast<Capture>().Select(c => new XElement("option", c.Value.ToLower()))).ToString(SaveOptions.DisableFormatting));

        public static List<FileLine> TrimLines(List<FileLine> lines)
        {
            return lines
                .Select(line => new FileLine(line.Number, line.Text.Trim()))
                .ToList();
        }
    }
}