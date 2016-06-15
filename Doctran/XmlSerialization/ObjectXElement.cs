// <copyright file="ObjectXElement.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.XmlSerialization
{
    using System;
    using System.Xml.Linq;
    using Parsing;

    public class ObjectXElement<TParsed> : IObjectXElement<TParsed>
        where TParsed : IFortranObject
    {
        public ObjectXElement(string name, XmlTraversalType xmlTraversalType = XmlTraversalType.HeadOrSubObjects)
        {
            this.XmlTraversalType = xmlTraversalType;
            this.Func = from => new XElement(name);
        }

        public ObjectXElement(Func<TParsed, XElement> func, XmlTraversalType xmlTraversalType = XmlTraversalType.HeadOrSubObjects)
        {
            this.Func = func;
            this.XmlTraversalType = xmlTraversalType;
        }

        public ObjectXElement(Func<TParsed, XElement> func, Predicate<TParsed> predicate, XmlTraversalType xmlTraversalType = XmlTraversalType.HeadOrSubObjects)
        {
            this.Func = func;
            this.Predicate = predicate;
            this.XmlTraversalType = xmlTraversalType;
        }

        public Type ForType => typeof(TParsed);

        public Func<TParsed, XmlCreationType> XmlCreationType { get; set; } = from => XmlSerialization.XmlCreationType.All;

        public XmlTraversalType XmlTraversalType { get; }

        private Func<TParsed, XElement> Func { get; }

        private Predicate<TParsed> Predicate { get; }

        public XElement Create(TParsed from)
        {
            if (this.Predicate != null && this.Predicate(from))
            {
                return null;
            }

            return this.Func(from);
        }

        public XmlCreationType GetXmlCreationType(TParsed from)
        {
            return this.XmlCreationType(from);
        }

        XElement IObjectXElement.Create(object from) => this.Create((TParsed)from);

        XmlCreationType IObjectXElement.GetXmlCreationType(object from) => this.GetXmlCreationType((TParsed)from);
    }
}