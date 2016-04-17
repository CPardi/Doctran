// <copyright file="MenuOptionAttribute.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Input.ProjectFileOptions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;
    using MarkdownSharp;
    using Options;
    using ParsingElements;
    using Utilitys;

    internal class MenuOptionAttribute : OptionListAttribute
    {
        public MenuOptionAttribute()
            : base("Menu", typeof(List<XElement>))
        {
        }

        public override object InformationToProperty(IInformation information, Type propertyType)
        {
            var value = information as IInformationValue;
            if (value == null)
            {
                throw new OptionReaderException(information.Lines.First().Number, information.Lines.Last().Number, $"'{information.Name}' must be a value type.");
            }

            try
            {
                var menuString = Regex.Replace(OtherUtils.ReadAllText(value.Value), @"(.*?\.)(?:md|markdown)", match => match.Groups[1].Value + "html");
                return XmlUtils.WrapAndParse("Menu", new Markdown().Transform(menuString));
            }
            catch (Exception e)
            {
                throw new OptionReaderException(information.Lines.First().Number, information.Lines.Last().Number, e.Message);
            }
        }
    }
}