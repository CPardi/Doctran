﻿//-----------------------------------------------------------------------
// <copyright file="OptionListBaseAttribute.cs" company="Christopher Pardi">
// Copyright © 2015 Christopher Pardi
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>
//-----------------------------------------------------------------------

namespace Doctran.Input.OptionsReaderCore
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Comments;

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
        ///     The concrete type that the property should be initialized to.
        /// </summary>
        public Type InitializationType { get; }

        /// <summary>
        ///     If true, then the property will be initialized as <see cref="InitializationType" />. If false, then
        ///     it will be assumned that the property is already initialized before parsing.
        /// </summary>
        public bool InitializeAsDefault { get; set; } = false;

        public abstract object MetaDataToProperty(IInformation metaData, Type propertyType);
    }
}