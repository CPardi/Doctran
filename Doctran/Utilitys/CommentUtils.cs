// <copyright file="CommentUtils.cs" company="Christopher Pardi">
//     Copyright � 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Utilitys
{
    using System.Text.RegularExpressions;

    public static class CommentUtils
    {
        public static bool DescEnd(string text)
        {
            return !Regex.IsMatch(text, @"^\s*!>");
        }

        public static bool DescStart(string text)
        {
            return Regex.IsMatch(text, @"^\s*!>");
        }

        public static bool DetailLine(string text)
        {
            return Regex.IsMatch(text, @"^\s*!>>");
        }

        public static bool InfoAtDepthStart(string text, int depth)
        {
            return Regex.IsMatch(text.Trim(), @"^\s*?!>{" + depth + @"}\s*?\w+?\s*?:");
        }

        public static int InfoDepth(string text)
        {
            return Regex.Match(text.Trim(), @"^\s*?!(>)+?\s*?\w+?\s*?:").Groups[1].Captures.Count;
        }

        public static bool InfoEnd(string text)
        {
            return !Regex.IsMatch(text, @"^\s*!>+");
        }

        public static bool InfoStart(string text)
        {
            return Regex.IsMatch(text.Trim(), @"^\s*?!>+?\s*?\w+?\s*?:");
        }

        public static bool NDescEnd(string text)
        {
            return !Regex.IsMatch(text, @"^\s*!>");
        }

        public static bool NDescStart(string text)
        {
            return Regex.IsMatch(text.Trim(), @"^\s*!>>?\s*\w[^\s]*\s*-.*");
        }
    }
}