//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

using Doctran.Fbase.Common;
using Doctran.Fbase.Comments;
using Doctran.BaseClasses;

namespace Doctran.OptionFile
{
    public class MenuFactory : IInformationFactory
    {
        public IEnumerable<IInformation> Create(int depth, string value, IEnumerable<FortranObject> subObjects, List<FileLine> lines)
        {
            var menu_html = Helper.GetMarkUpFile(Directory.GetCurrentDirectory() + EnvVar.slash, value);
            // Change any .md extensions to .html
            string menu_string = Regex.Replace(menu_html.Item2, @"(.*?\.)(?:md|markdown)", new MatchEvaluator(match => match.Groups[1].Value + "html"));

            yield return new Menu(depth, menu_string, subObjects, lines);
        }
    }

    public class Menu : XInformation, IInformation
    {
        public Menu(int depth, string menuString, IEnumerable<FortranObject> sub_objects, List<FileLine> lines)
            : base(depth, "Menu", menuString, sub_objects, lines) { }
    }

}