//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Doctran.Input.OptionFile
{
    using System.Collections.Generic;
    using System.Linq;
    using Comments;
    using Helper;
    using Parsing;
    using Parsing.FortranObjects;
    using Utilitys;

    public class UserPageFactory : IInformationFactory
    {
        public IEnumerable<IInformation> Create(int depth, string value, IEnumerable<FortranObject> subObjects, List<FileLine> lines)
        {
            var paths = new PathList(false) { value.Trim() };

            return paths
                .Select(p => HelperUtils.GetMarkUpFile(string.Empty, p))
                .Select(htmlFile => new UserPage(depth, htmlFile.Item1, htmlFile.Item2, lines));
        }
    }

    public class UserPage : XInformation
    {
        public UserPage(int depth, string path, string content, List<FileLine> lines)
            : base(depth
                , "UserPage"
                , null
                , new List<XInformation> { new XInformation(depth + 1, "Path", path, lines), new XInformation(depth + 1, "Content", content, lines) }
                , lines)
        {
        }
    }
}