//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Doctran.Fbase.Comments
{
    public static class CommentDefinitions
    {
        public static bool DescStart(String text) { return Regex.IsMatch(text, @"^\s*!>"); }

        public static bool DescEnd(String text) { return !Regex.IsMatch(text, @"^\s*!>"); }

        public static bool NDescStart(String text) { return Regex.IsMatch(text.Trim(), @"!>\s*\w[^\s]*\s*-.*"); }

        public static bool NDescEnd(String text) { return !Regex.IsMatch(text, @"^\s*!>"); }

        public static bool InfoStart(String text) { return Regex.IsMatch(text.Trim(), @"^!>\s*\w+?\s*:"); }

        public static bool InfoEnd(String text) { return !Regex.IsMatch(text, @"^\s*!>>"); }

        public static bool DetailLine(String text) { return Regex.IsMatch(text, @"^\s*!>>"); }
    }
}