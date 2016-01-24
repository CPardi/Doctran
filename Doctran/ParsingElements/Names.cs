namespace Doctran.ParsingElements
{
    using System;
    using System.Collections.Generic;
    using FortranObjects;

    public static class Names
    {
        private static readonly Dictionary<Type, string> TypeNames = new Dictionary<Type, string>
        {
            { typeof(InformationGroup), "Information Group" },
            { typeof(IInformationGroup), "Information Group" },
            { typeof(InformationValue), "Information Value" },
            { typeof(IInformationValue), "Information Value" },
            { typeof(IInformation), "Information" },
            { typeof(NamedDescription), "Named Description" },
            { typeof(IDescription), "Description" },
            { typeof(ISourceFile), "Source File" },
            { typeof(SourceFile), "Source File" },
        };

        public static void AddName(Type type, string name)
        {
            TypeNames.Add(type, name);
        }

        public static void AddNames(Dictionary<Type, string> typeNames)
        {
            foreach (var tn in typeNames)
            {
                Names.AddName(tn.Key, tn.Value);
            }
        }

        public static string OfType(Type type) => TypeNames.ContainsKey(type) ? TypeNames[type] : type.Name;
    }
}