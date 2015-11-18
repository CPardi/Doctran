namespace Doctran.Utilitys
{
    using System;
    using System.Xml;
    using System.Xml.Linq;

    public static class XmlUtils
    {
        public static XDocument ToXDocument(this XmlDocument document)
        {
            return document.ToXDocument(LoadOptions.None);
        }

        public static XDocument ToXDocument(this XmlDocument document, LoadOptions options)
        {
            using (var reader = new XmlNodeReader(document))
            {
                return XDocument.Load(reader, options);
            }
        }

        public static XElement ToXElement(this DateTime value)
        {
            return new XElement("DateTime", value.ToString("o"));
        }
    }
}