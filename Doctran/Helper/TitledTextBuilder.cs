// <copyright file="TitledTextBuilder.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    internal class TitledTextBuilder
    {
        private readonly StringBuilder _sb = new StringBuilder();

        private readonly List<string> _texts = new List<string>();

        private readonly List<string> _titles = new List<string>();

        public int LeftMargin { get; set; } = 1;

        public int RightMargin { get; set; } = 1;

        private static IEnumerable<char> Seperators => new[] { ' ', ',', '\\', '/', ';', ':', '|', '[', '(', ')' };

        public void Append(string title, string text)
        {
            _titles.Add(title);
            _texts.Add(text);
        }

        public override string ToString()
        {
            var titleWidth = _titles.Max(t => t.Length) + 3 + this.LeftMargin;

            for (var i = 0; i < _titles.Count; i++)
            {
                var title = _titles[i];
                var text = _texts[i];

                _sb.Append((new string(' ', this.LeftMargin) + title + ": ").PadRight(titleWidth));
                this.AddText(titleWidth, text);
                _sb.Append(Environment.NewLine);
            }

            return _sb.ToString();
        }

        private void AddText(int titleWidth, string text)
        {
            var indentString = new string(' ', titleWidth);
            var indentWidth = indentString.Length;

            var writableWidth = Math.Min(Console.BufferWidth, Console.LargestWindowWidth) - this.RightMargin;

            var pos = indentWidth;
            foreach (var t in Regex.Split(text, $@"(?<=[{Regex.Escape(string.Concat(Seperators))}])").Where(t => t != string.Empty))
            {
                pos += t.Length;

                if (pos >= writableWidth)
                {
                    _sb.Append(Environment.NewLine);
                    _sb.Append(indentString);
                    pos = indentWidth + t.Length;
                }

                _sb.Append(t);
            }
        }
    }
}