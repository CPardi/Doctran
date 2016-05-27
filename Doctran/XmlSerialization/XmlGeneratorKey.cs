namespace Doctran.XmlSerialization
{
    using System;

    internal class XmlGeneratorKey
    {
        public XmlGeneratorKey(Type type, XmlTraversalType xmlTraversalType)
        {
            this.Type = type;
            this.XmlTraversalType = xmlTraversalType;
        }

        public Type Type { get; }

        public XmlTraversalType XmlTraversalType { get; }
    }
}