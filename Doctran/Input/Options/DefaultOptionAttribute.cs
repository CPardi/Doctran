// <copyright file="DefaultOptionAttribute.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Input.Options
{
    using System;
    using System.Linq;
    using ParsingElements;
    using Utilitys;

    public class DefaultOptionAttribute : OptionListBaseAttribute, IDefaultOptionAttribute
    {
        private readonly string[] _allowedNames;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultOptionAttribute" /> class.
        /// </summary>
        /// <param name="initializationType">The concrete type the string value of the information should be cast to.</param>
        /// <param name="allowedNames">
        ///     Names of options that can be allowed to be passed through if <see cref="Strict" /> is set to
        ///     true.
        /// </param>
        public DefaultOptionAttribute(Type initializationType, params string[] allowedNames)
            : base(initializationType)
        {
            _allowedNames = allowedNames;
        }

        public bool Strict { get; set; } = false;

        public override object InformationToProperty(IInformation information, Type propertyType)
        {
            var infoV = information as IInformationValue;

            if (infoV == null)
            {
                throw new OptionReaderException(information.Lines.First().Number, information.Lines.Last().Number, $"'{information.Name}' can only be a value type and not a group.");
            }

            if (this.Strict && !_allowedNames.Contains(infoV.Name))
            {
                throw new OptionReaderException(information.Lines.First().Number, information.Lines.Last().Number, $"'{information.Name}' is not a recognised option.");
            }

            return infoV.Value.ToIConvertable(propertyType);
        }
    }
}