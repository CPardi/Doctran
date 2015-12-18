// <copyright file="LanguageParserManager.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Plugins
{
    using System;
    using System.Collections.Generic;
    using Reporting;

    public static class ParserManager
    {
        private static readonly Dictionary<string, ILanguageParser> ParsersByExtension = new Dictionary<string, ILanguageParser>();

        private static readonly Dictionary<string, ILanguageParser> ParsersByIdentifier = new Dictionary<string, ILanguageParser>();

        public static void RegisterLanguageParser(string identifier, string extension, ILanguageParser languageParser)
        {
            ParsersByIdentifier.Remove(identifier);
            ParsersByIdentifier.Add(identifier, languageParser);

            ParsersByExtension.Remove(extension);
            ParsersByExtension.Add(extension, languageParser);
        }

        public static ILanguageParser GetParserByExtension(string extension)
        {
            ILanguageParser languageParser;
            if (ParsersByExtension.TryGetValue(extension, out languageParser))
            {
                return languageParser;
            }

            throw new ApplicationException($"A parser is not defined for the file extension '{extension}'.");
        }

        public static ILanguageParser GetParserByIdentifier(string identifier)
        {
            ILanguageParser languageParser;
            if (ParsersByIdentifier.TryGetValue(identifier, out languageParser))
            {
                return languageParser;
            }

            throw new ApplicationException($"A parser is not defined for the language identifier '{identifier}'.");
        }
    }
}