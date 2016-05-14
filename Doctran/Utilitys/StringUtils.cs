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
        private static readonly Random Random = new Random();

        public static string ConvertFromFileLineList(List<FileLine> lines)
            => string.Concat(lines.Select((line, index) => index == 0 ? line.Text : $"\n{line.Text}"));

        public static List<FileLine> ConvertToFileLineList(string linesString)
        {
            return
                linesString
                    .Replace("\r\n", "\n").Replace("\r", "\n") // Normalize line endings.
                    .Split('\n')
                    .Select((l, i) => new FileLine(i + 1, l)).ToList();
        }

        public static string DelimiteredConcat(this IEnumerable<string> @this, string delimiter)
        {
            var stringList = @this.ToList();
            if (stringList.Count == 1)
            {
                return stringList.First();
            }

            return string.Concat(
                stringList
                    .Select(
                        (str, position) => str + (position + 1 < stringList.Count ? delimiter : string.Empty)));
        }

        public static string DelimiteredConcat(this IEnumerable<string> @this, string delimiter, string lastDelimiter)
        {
            var stringList = @this.ToList();
            if (stringList.Count == 1)
            {
                return stringList.First();
            }

            return string.Concat(
                stringList
                    .Select(
                        (str, position) =>
                            (position + 1 == stringList.Count ? lastDelimiter : string.Empty) +
                            str +
                            (position + 2 < stringList.Count ? delimiter : string.Empty)));
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

                if (currentIndex == text.Length - 1)
                {
                    delimiteredText.Add(text.Substring(prevIndex, currentIndex - prevIndex + 1).Trim());
                }

                currentIndex++;
            }

            return delimiteredText;
        }

        public static List<string> SplitExceptChars(string text, char split, Tuple<char, char>[] pairs) => SplitExceptChars(text, new[] { split.ToString() }, pairs);

        public static List<string> SplitExceptChars(string text, char[] split, Tuple<char, char>[] pairs) => SplitExceptChars(text, split.Select(s => s.ToString()), pairs);

        public static List<string> SplitExceptChars(string text, IEnumerable<string> split, Tuple<char, char>[] pairs)
        {
            var delimiteredText = new List<string>();

            var pairDepth = new int[pairs.Length];

            var prevIndex = 0;
            var charArray = text.ToCharArray();
            var splitArray = split.OrderByDescending(str => str.Length).ToArray();

            var charNum = 0;
            while (charNum < charArray.Length)
            {
                var aChar = charArray[charNum];

                // Search through each pair for split exception begining.
                for (var i = 0; i < pairs.Length; i++)
                {
                    // Check for ending of identical exception pair.
                    if (aChar == pairs[i].Item1 && pairs[i].Item1 == pairs[i].Item2 && pairDepth[i] == 1)
                    {
                        pairDepth[i]--;
                        break;
                    }

                    // Check for exception pair begining.
                    if (aChar == pairs[i].Item1)
                    {
                        pairDepth[i]++;
                        break;
                    }

                    // CHeck for differing exception pair ending.
                    if (aChar == pairs[i].Item2)
                    {
                        pairDepth[i]--;
                        break;
                    }
                }

                // If we are not within a exception pair block then search for split character.
                if (pairDepth.All(d => d == 0))
                {
                    // Split array should be in size order, so the first match will then be the longest match. Make sure there is enough of the string left and
                    // see if the next character match any of the split strings.
                    var matchString = splitArray.FirstOrDefault(str => text.Length - (charNum + str.Length) >= 0 && str == text.Substring(charNum, str.Length));
                    if (matchString != null)
                    {
                        delimiteredText.Add(text.Substring(prevIndex, charNum - prevIndex));
                        prevIndex = charNum + matchString.Length;
                        charNum = charNum + matchString.Length - 1;
                    }
                }

                // If we are at the last character then add the final substring.
                if (charNum == text.Length - 1)
                {
                    delimiteredText.Add(text.Substring(prevIndex, charNum - prevIndex + 1));
                    break;
                }

                charNum++;
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
        /// <returns>The location string.</returns>
        public static string LocationString(int start, int end, string path)
        {
            return
                start == end
                    ? $"At line {start} of '{path}'."
                    : $"Within lines {start} to {end} of '{path}'.";
        }

        public static string NoWhitespace(string text)
        {
            return Regex.Replace(text, @"\s+", string.Empty);
        }

        public static string RemoveInlineComment(string s)
        {
            return s.Split('!')[0].Trim();
        }

        /// <summary>
        ///     Convert a string value to a specified type.
        /// </summary>
        /// <param name="this">The converted object.</param>
        /// <param name="propertyType">
        ///     The type to convert the meta-data to. Must have interface <see cref="IConvertible" />
        /// </param>
        /// <returns>An IConverable object.</returns>
        /// <exception cref="ArgumentException">
        ///     Thrown if <paramref name="propertyType" /> does not have teh
        ///     <see cref="IConvertible" /> interface.
        /// </exception>
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

        public static string ToRandomCase(this string @string)
            => new string(@string.ToCharArray().Select(aChar => Random.Next(2) == 0 ? char.ToUpper(aChar) : char.ToLower(aChar)).ToArray());

        public static string ToUpperFirstLowerRest(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            return char.ToUpper(s[0]) + s.Substring(1).ToLower();
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