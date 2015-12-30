//-----------------------------------------------------------------------
// <copyright file="XmlDefaultOptionAttribute.cs" company="Christopher Pardi">
// Copyright © 2015 Christopher Pardi
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>
//-----------------------------------------------------------------------

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
    internal class XmlDefaultOptionAttribute : DefaultOptionAttribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="XmlDefaultOptionAttribute" /> class.
        /// </summary>
        public XmlDefaultOptionAttribute()
            : base(typeof(List<XElement>))
        {
        }

        /// <summary>
        ///     Converts the intermediary <see cref="IMetaData" /> to a <see cref="XElement" />.
        /// </summary>
        /// <param name="metaData">The meta-data to be converted.</param>
        /// <param name="propertyType">The actual type of the property this attribute was applied to.</param>
        /// <returns>The result of the conversion.</returns>
        /// <exception cref="InvalidPropertyTypeException">
        ///     Thrown when the property this attribute was applied to is not of type
        ///     <see cref="XElement" />.
        /// </exception>
        public override object MetaDataToProperty(IInformation metaData, Type propertyType)
        {
            if (propertyType != typeof(XElement))
            {
                throw new InvalidPropertyTypeException("MetaDataToProperty", typeof(XElement), propertyType);
            }

            return ToXml(metaData);
        }

        /// <summary>
        ///     Converts an <see cref="IMetaData" /> to an <see cref="XElement" />.
        /// </summary>
        /// <param name="metaData">The meta-data to be converted.</param>
        /// <returns>The result of the conversion.</returns>
        private static XElement ToXml(IInformation metaData)
        {
            var value = metaData as IInformationValue;
            if (value != null)
            {
                return new XElement(value.Name, value.Value);
            }

            var group = (IInformationGroup) metaData;
            return new XElement(group.Name, group.SubObjects.OfType<IInformation>().Select(ToXml));
        }
    }
}