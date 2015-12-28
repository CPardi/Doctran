//-----------------------------------------------------------------------
// <copyright file="OptionAttribute.cs" company="Christopher Pardi">
// Copyright © 2015 Christopher Pardi
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>
//-----------------------------------------------------------------------

namespace Doctran.Input.OptionsReaderCore
{
    using System;
    using System.Linq;
    using Comments;
    using Parsing;
    using Utilitys;

    [AttributeUsage(AttributeTargets.Property)]
    public class OptionAttribute : Attribute, IOptionAttribute
    {
        public OptionAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; }

        public object DefaultValue { get; set; }

        public virtual object MetaDataToProperty(IInformation metaData, Type propertyType)
        {
            var value = metaData as IInformationValue;
            if (value == null)
            {
                throw new OptionReaderException(metaData.Lines.First().Number, metaData.Lines.Last().Number, $"'{metaData.Name}' can only be a value type and not a group.");
            }

            try
            {
                return value.Value.ToIConvertable(propertyType);
            }
            catch (FormatException e)
            {
                throw new OptionReaderException(metaData.Lines.First().Number, metaData.Lines.Last().Number, e.Message);
            }
        }
    }
}