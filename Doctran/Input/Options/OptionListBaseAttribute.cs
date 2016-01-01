// <copyright file="OptionListBaseAttribute.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Input.Options
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using ParsingElements;

    [AttributeUsage(AttributeTargets.Property)]
    public abstract class OptionListBaseAttribute : Attribute, IOptionListAttribute
    {
        protected OptionListBaseAttribute(Type initializationType)
        {
            if (initializationType.IsAbstract)
            {
                throw new ArgumentException($"'{nameof(initializationType)}' is an abstract type ({initializationType}).");
            }

            if (initializationType.GetInterface(typeof(IEnumerable<>).Name) == null)
            {
                throw new ArgumentException($"'{nameof(initializationType)}' is an abstract type ({initializationType}).");
            }

            if (initializationType.GetInterface(typeof(IList).Name) == null)
            {
                throw new ArgumentException($"'{nameof(initializationType)}' must derive from '{typeof(IList).Name}'.");
            }

            this.InitializationType = initializationType;
        }

        /// <summary>
        ///     Gets the concrete type that the property will be initialized to.
        /// </summary>
        public Type InitializationType { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether will be initialized by the <see cref="OptionsReader{TOptions}"/>.
        /// </summary>
        public bool InitializeAsDefault { get; set; } = false;

        public abstract object MetaDataToProperty(IInformation metaData, Type propertyType);
    }
}