namespace Doctran.Utilitys
{
    using System;
    using System.Linq;

    public static class ReflectionUtils
    {
        public static T GetAssemblyAttribute<T>(this System.Reflection.Assembly ass) where T : Attribute
        {
            var attributes = ass.GetCustomAttributes(typeof(T), false);
            return 
                attributes.Length != 0 
                    ? attributes.OfType<T>().Single() 
                    : null;
        }
    }
}