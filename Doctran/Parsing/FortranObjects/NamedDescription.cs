//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Doctran.Parsing.FortranObjects
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Helper;
    using Parsing;

    public class NamedDescription : List<FortranObject>
    {
        public NamedDescription(List<FileLine> lines)
        {
            Match aMatch = Regex.Match(lines[0].Text, @"!>\s*(\w.*)\s*-(.*)");
            string name = aMatch.Groups[1].Value.Trim();
            StringBuilder basic = new StringBuilder(aMatch.Groups[2].Value.Trim()).Append(
                string.Concat(
                from line in lines.Skip(1)
                where Regex.IsMatch(line.Text, @"^\s*!>")
                where !Regex.IsMatch(line.Text, @"^\s*!>>")
                select " " + line.Text.Trim().Substring(3)));

            string detailed = Description.MergeLines(lines);

            this.Add(new Description(name, basic.ToString(), detailed, lines));
            
        }
    }

}