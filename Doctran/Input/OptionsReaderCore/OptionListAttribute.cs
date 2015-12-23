//-----------------------------------------------------------------------
// <copyright file="OptionListAttribute.cs" company="Christopher Pardi">
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
    using Utilitys;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class OptionListAttribute : OptionListBaseAttribute, IOptionAttribute
    {
        public OptionListAttribute(string name, Type initializationType)
            : base(initializationType)
        {
            this.Name = name;
        }

        public string Name { get; }

        public override object MetaDataToProperty(IInformation metaData, Type propertyType)
        {
            var infoV = metaData as IInformationValue;
            if (infoV == null)
            {
                throw new ParserException(metaData.Lines.First().Number, metaData.Lines.Last().Number, $"'{metaData.Name}' can only be a value type and not a group.");
            }

            return infoV.Value.ToIConvertable(propertyType);
        } 
    }
}