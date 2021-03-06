﻿// <copyright file="XmlUtils.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

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

        public static XElement ToXElement(this XmlNode node)
        {
            var xDoc = new XDocument();
            using (var xmlWriter = xDoc.CreateWriter())
            {
                node.WriteTo(xmlWriter);
            }

            return xDoc.Root;
        }

        public static XmlNode ToXmlNode(this XElement element)
        {
            using (var xmlReader = element.CreateReader())
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlReader);
                return xmlDoc;
            }
        }

        public static XElement WrapAsCData(string name, string text)
        {
            return new XElement(name, new XCData(text));
        }

        public static XElement WrapAndParse(string name, string text)
        {
            return XElement.Parse($"<{name}>{text}</{name}>", LoadOptions.PreserveWhitespace);
        }

        public static XElement WrapAndParse(string name, string attribute, string text)
        {
            return XElement.Parse($"<{name} {attribute}>{text}</{name}>", LoadOptions.PreserveWhitespace);
        }
    }
}