﻿//-----------------------------------------------------------------------
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
    using Parsing.BuiltIn.FortranObjects;
    using Utilitys;

    public class ParserException : ApplicationException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ParserException" /> class.
        /// </summary>
        /// <param name="startLine">The number of the first line on which the incorrect text was found.</param>
        /// <param name="endLine">The number of the last line on which the incorrect text was found.</param>
        /// <param name="message">A description of the exception that occured.</param>
        public ParserException(int startLine, int endLine, string message)
            : base(message)
        {
            this.StartLine = startLine;
            this.EndLine = endLine;
        }

        /// <summary>
        ///     The number of the last line on which the incorrect text was found.
        /// </summary>
        public int EndLine { get; }

        /// <summary>
        ///     The number of the first line on which the incorrect text was found.
        /// </summary>
        public int StartLine { get; }
    }

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
                throw new ParserException(metaData.Lines.First().Number, metaData.Lines.Last().Number, $"'{metaData.Name}' can only be a value type and not a group.");
            }

            try
            {
                return value.Value.ToIConvertable(propertyType);
            }
            catch (FormatException e)
            {
                throw new ConversionException(metaData.Lines.First().Number, metaData.Lines.Last().Number, metaData.Name, e.Message);
            }
        }
    }
}