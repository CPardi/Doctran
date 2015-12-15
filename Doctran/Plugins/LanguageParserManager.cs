// <copyright file="LanguageParserManager.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Plugins
{
    using System.Collections.Generic;

    public static class LanguageParserManager
    {
        private static readonly Dictionary<string, ILanguageParser> LanguageParsers = new Dictionary<string, ILanguageParser>();

        public static void RegisterLanguageParser(string extension, ILanguageParser languageParser)
        {
            LanguageParsers.Remove(extension);
            LanguageParsers.Add(extension, languageParser);
        }

        public static bool TryGetLanguage(string extension, out ILanguageParser languageParser)
        {
            return LanguageParsers.TryGetValue(extension, out languageParser);
        }
    }
}