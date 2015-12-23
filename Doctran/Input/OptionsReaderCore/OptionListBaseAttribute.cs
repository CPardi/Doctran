//-----------------------------------------------------------------------
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
    using Parsing.BuiltIn.FortranObjects;    

    [AttributeUsage(AttributeTargets.Property)]
    public abstract class OptionListBaseAttribute : Attribute, IOptionListAttribute
    {
        protected OptionListBaseAttribute(Type initializationType)
        {
            if (initializationType.IsAbstract)
            {
                throw new ArgumentException("Cannot be of abstract type.", nameof(initializationType));
            }

            if (initializationType.GetInterface(typeof(IEnumerable<>).Name) == null)
            {
                throw new ArgumentException($"Must derive from base type '{typeof(IEnumerable<>).Name}'.", nameof(initializationType));
            }

            if (initializationType.GetInterface(typeof(IList).Name) == null)
            {
                throw new ArgumentException($"Must derive from base type '{typeof(IList).Name}'.", nameof(initializationType));
            }

            this.InitializationType = initializationType;
        }

        public Type InitializationType { get; }

        public bool InitializeAsDefault { get; set; } = false;

        public abstract object MetaDataToProperty(IInformation metaData, Type propertyType);
    }
}