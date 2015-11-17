namespace Doctran
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
