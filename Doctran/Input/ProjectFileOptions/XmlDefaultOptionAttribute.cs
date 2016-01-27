// <copyright file="XmlDefaultOptionAttribute.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Input.ProjectFileOptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Options;
    using ParsingElements;

    /// <summary>
    ///     Defines a property that holds XML data that mirrors a project file's group-value structure.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    internal class XmlDefaultOptionAttribute : OptionListBaseAttribute, IDefaultOptionAttribute
    {
        private readonly string[] _allowedNames;

        /// <summary>
        ///     Initializes a new instance of the <see cref="XmlDefaultOptionAttribute" /> class.
        /// </summary>
        /// <param name="allowedNames">Names of options that can be allowed to be passed through if <see cref="Strict"/> is set to true.</param>
        public XmlDefaultOptionAttribute(params string[] allowedNames)
            : base(typeof(List<XElement>))
        {
            _allowedNames = allowedNames;
        }

        public bool Strict { get; set; }

        /// <summary>
        ///     Converts the intermediary <see cref="IInformation" /> to a <see cref="XElement" />.
        /// </summary>
        /// <param name="information">The meta-data to be converted.</param>
        /// <param name="propertyType">The actual type of the property this attribute was applied to.</param>
        /// <returns>The result of the conversion.</returns>
        /// <exception cref="InvalidPropertyTypeException">
        ///     Thrown when the property this attribute was applied to is not of type
        ///     <see cref="XElement" />.
        /// </exception>
        public override object InformationToProperty(IInformation information, Type propertyType)
        {
            if (propertyType != typeof(XElement))
            {
                throw new InvalidPropertyTypeException(nameof(this.InformationToProperty), typeof(XElement), propertyType);
            }

            if (this.Strict && !_allowedNames.Contains(information.Name))
            {
                throw new OptionReaderException(information.Lines.First().Number, information.Lines.Last().Number, $"'{information.Name}' is not a recognised option.");
            }

            return ToXml(information);
        }

        /// <summary>
        ///     Converts an <see cref="IInformation" /> to an <see cref="XElement" />.
        /// </summary>
        /// <param name="information">The meta-data to be converted.</param>
        /// <returns>The result of the conversion.</returns>
        private static XElement ToXml(IInformation information)
        {
            var value = information as IInformationValue;
            if (value != null)
            {
                return new XElement(value.Name, value.Value);
            }

            var group = (IInformationGroup)information;
            return new XElement(group.Name, group.SubObjects.OfType<IInformation>().Select(ToXml));
        }
    }
}