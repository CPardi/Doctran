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
    using Utilitys;

    public class InterfaceXElements<TParsed> : IInterfaceXElements, IInterfaceXElements<TParsed>
        where TParsed : IFortranObject
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="InterfaceXElements{TParsed}" /> class.
        ///     Specifies that an interface should generate a single node with the specified name and value.
        /// </summary>
        /// <param name="name">The XML node's name.</param>
        /// <param name="getValue">The XML node's value.</param>
        public InterfaceXElements(string name, Func<TParsed, string> getValue)
            : this(name, getValue, parsed => true)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="InterfaceXElements{TParsed}" /> class.
        ///     Specifies that an interface should only generate a single node with the specified name and value, when a predicate
        ///     is true.
        /// </summary>
        /// <param name="name">The XML node's name.</param>
        /// <param name="getValue">The XML node's value.</param>
        /// <param name="predicate">If this condition is met, then the specified XML will be generated.</param>
        public InterfaceXElements(string name, Func<TParsed, string> getValue, Predicate<TParsed> predicate)
            : this(from => CollectionUtils.Singlet(new XElement(name, getValue(from))), predicate)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="InterfaceXElements{TParsed}" /> class.
        ///     Specifies that an interface should generate an enumeration of nodes as specified by <paramref name="func" />.
        /// </summary>
        /// <param name="func">Maps an interface to an enumeration of XML nodes.</param>
        public InterfaceXElements(Func<TParsed, IEnumerable<XElement>> func)
            : this(func, parsed => true)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="InterfaceXElements{TParsed}" /> class.
        ///     Specifies that an interface should generate an enumeration of nodes as specified by <paramref name="func" />, when
        ///     a predicate is true.
        /// </summary>
        /// <param name="func">Maps an interface to an enumeration of XML nodes.</param>
        /// <param name="predicate">If this condition is met, then the specified XML will be generated.</param>
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

        public IEnumerable<XElement> Create(TParsed from) => this.Func(@from);

        public bool ShouldCreate(TParsed from) => this.Predicate(@from);

        IEnumerable<XElement> IInterfaceXElements.Create(object from) => this.Create((TParsed)from);

        bool IInterfaceXElements.ShouldCreate(object from) => this.Predicate((TParsed)from);
    }
}