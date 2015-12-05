namespace Doctran.Utilitys
{
    using System.Collections.Generic;

    public static class CollectionUtils
    {
        public static IEnumerable<T> Repeat<T>(int times, T tIn)
        {
            for (var index = 0; index < times; index++)
            {
                yield return tIn;
            }
        }

        public static IEnumerable<T> Empty<T>()
        {
            return new List<T>();
        }

        public static IEnumerable<T> Singlet<T>(T tInstance)
        {
            yield return tInstance;
        }
    }
}