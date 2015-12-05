﻿//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Doctran.Input.OptionFile
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using Comments;
    using Helper;
    using Parsing;
    using Parsing.BuiltIn.FortranObjects;
    using Utilitys;

    public class MenuFactory : IInformationFactory
    {
        public IEnumerable<IInformation> Create(int depth, string value, IEnumerable<IFortranObject> subObjects, List<FileLine> lines)
        {
            var menuHtml = OtherUtils.GetMarkUpFile(Directory.GetCurrentDirectory() + EnvVar.Slash, value);
            // Change any .md extensions to .html
            var menuString = Regex.Replace(menuHtml.Item2, @"(.*?\.)(?:md|markdown)", new MatchEvaluator(match => match.Groups[1].Value + "html"));

            yield return new Menu(depth, menuString, subObjects, lines);
        }
    }

    public class Menu : XInformation
    {
        public Menu(int depth, string menuString, IEnumerable<IFortranObject> subObjects, List<FileLine> lines)
            : base(depth, "Menu", menuString, subObjects, lines) { }
    }

}