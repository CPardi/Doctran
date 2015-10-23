namespace Doctran
{
    using System.Collections.Generic;

    public static class LanguageManager
    {
        private static readonly Dictionary<string, ILanguage> Languages = new Dictionary<string, ILanguage>();

        public static void RegisterLanguage(string extension, ILanguage language)
        {
            Languages.Remove(extension);
            Languages.Add(extension, language);
        }

        public static bool TryGetLanguage(string extension, out ILanguage language)
        {
            return Languages.TryGetValue(extension, out language);
        }
    }
}
