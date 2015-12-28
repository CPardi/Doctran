// <copyright file="StringUtils.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Utilitys
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Helper;

    public static class StringUtils
    {
        public static string ConvertFromFileLineList(List<FileLine> lines)
            => string.Concat(lines.Select((line, index) => index == 0 ? line.Text : $"\n{line.Text}"));

        public static List<FileLine> ConvertToFileLineList(string linesString)
        {
            var lines = new List<FileLine>();
            lines.AddRange(
                linesString
                    .Split('\n')
                    .Select((l, i) => new FileLine(i, l))
                );
            return lines;
        }

        public static List<string> DelimiterExceptBrackets(string text, char delimiter)
        {
            var delimiteredText = new List<string>();
            var withinBracketsDepth = 0;
            int prevIndex = 0, currentIndex = 0;
            foreach (var aChar in text)
            {
                if (aChar == '(' | aChar == '[')
                {
                    withinBracketsDepth++;
                }
                if (aChar == ')' | aChar == ']')
                {
                    withinBracketsDepth--;
                }
                if (withinBracketsDepth == 0 & aChar == delimiter)
                {
                    delimiteredText.Add(text.Substring(prevIndex, currentIndex - prevIndex).Trim());
                    prevIndex = currentIndex + 1;
                }
                if (currentIndex == text.Count() - 1)
                {
                    delimiteredText.Add(text.Substring(prevIndex, currentIndex - prevIndex + 1).Trim());
                }
                currentIndex++;
            }
            return delimiteredText;
        }

        public static List<string> DelimiterExceptQuotes(string text, char delimiter)
        {
            var delimiteredText = new List<string>();
            var sQuotes = false;
            var dQuotes = false;
            int prevIndex = 0, currentIndex = 0;
            foreach (var aChar in text)
            {
                if (!(sQuotes | dQuotes))
                {
                    sQuotes = aChar == '\'';
                    dQuotes = aChar == '"';
                }
                else
                {
                    if (sQuotes)
                    {
                        sQuotes = aChar != '\'';
                    }
                    if (dQuotes)
                    {
                        dQuotes = aChar != '"';
                    }
                }

                if (!(sQuotes | dQuotes) && aChar == delimiter)
                {
                    delimiteredText.Add(text.Substring(prevIndex, currentIndex - prevIndex).Trim());
                    prevIndex = currentIndex + 1;
                }

                if (currentIndex == text.Length - 1)
                {
                    delimiteredText.Add(text.Substring(prevIndex, currentIndex - prevIndex + 1).Trim());
                }

                currentIndex++;
            }

            return delimiteredText;
        }

        public static bool IsNullOrEmpty(this string @this)
        {
            return string.IsNullOrEmpty(@this);
        }

        /// <summary>
        ///     Return a string describing the location of lines within a file path.
        /// </summary>
        /// <param name="start">The first line to locate.</param>
        /// <param name="end">The last line to locate.</param>
        /// <param name="path">The file path of the file.</param>
        /// <returns></returns>
        public static string LocationString(int start, int end, string path)
        {
            return
                start == end
                    ? $"At line {start} of '{path}'."
                    : $"Within lines {start} to {end} of '{path}'.";
        }

        public static string NoWhitespace(string text)
        {
            return Regex.Replace(text, @"\s+", "");
        }

        public static string RemoveInlineComment(string s)
        {
            return s.Split('!')[0].Trim();
        }

        /// <summary>
        ///     If convertable, a string value is converted to the specified type.
        /// </summary>
        /// <param name="this">The converted object.</param>
        /// <param name="propertyType">The type to convert the meta-data to.</param>
        public static object ToIConvertable(this string @this, Type propertyType)
        {
            if (propertyType.GetInterface(typeof(IConvertible).Name) == null)
            {
                throw new ArgumentException(
                    $"Must derive from base type '{typeof(IConvertible).Name}'",
                    nameof(propertyType));
            }

            return Convert.ChangeType(@this, propertyType);
        }

        public static string ToUpperFirstLowerRest(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static string ValidName(string name)
        {
            return name.Replace('+', 'p')
                .Replace('-', 'm')
                .Replace('/', 'd')
                .Replace('\\', 'V')
                .Replace('*', 't')
                .Replace('=', 'e')
                .Replace('.', 'o')
                .Replace('<', 'l')
                .Replace('>', 'g');
        }
    }
}