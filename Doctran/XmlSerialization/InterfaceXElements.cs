// <copyright file="InterfaceXElements.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.XmlSerialization
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;
    using Parsing;

    public class InterfaceXElements<TParsed> : IInterfaceXElements, IInterfaceXElements<TParsed>
        where TParsed : IFortranObject
    {
        public InterfaceXElements(Func<TParsed, IEnumerable<XElement>> func)
            : this(func, parsed => true)
        {
        }

        public InterfaceXElements(Func<TParsed, IEnumerable<XElement>> func, Predicate<TParsed> predicate)
        {
            if (!typeof(TParsed).IsInterface)
            {
                throw new TypeParameterException($"'{nameof(TParsed)}' must be an interface, not an implementation. The specified " +
                                                 $"generic type parameter is the class '{typeof(TParsed).Name}'.");
            }

            this.Func = func;
            this.Predicate = predicate;
        }

        public Type ForType => typeof(TParsed);

        private Func<TParsed, IEnumerable<XElement>> Func { get; }

        private Predicate<TParsed> Predicate { get; }

        public IEnumerable<XElement> Create(TParsed from) => this.Func(from);

        public bool ShouldCreate(TParsed from) => this.Predicate(from);

        IEnumerable<XElement> IInterfaceXElements.Create(object from) => this.Create((TParsed)from);

        bool IInterfaceXElements.ShouldCreate(object from) => this.Predicate((TParsed)from);
    }
}