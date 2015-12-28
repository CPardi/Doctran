//-----------------------------------------------------------------------
// <copyright file="UserPageOptionAttribute.cs" company="Christopher Pardi">
// Copyright © 2015 Christopher Pardi
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>
//-----------------------------------------------------------------------

namespace Doctran.Input.ProjectFileCore
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;
    using Comments;
    using MarkdownSharp;
    using OptionsReaderCore;
    using Parsing;
    using Utilitys;

    internal class MenuOptionAttribute : OptionListAttribute
    {
        public MenuOptionAttribute()
            : base("Menu", typeof(List<XElement>))
        {
        }

        public override object MetaDataToProperty(IInformation metaData, Type propertyType)
        {
            var value = metaData as IInformationValue;
            if (value == null)
            {
                throw new OptionReaderException(metaData.Lines.First().Number, metaData.Lines.Last().Number, $"'{metaData.Name}' must be a value type.");
            }

            try
            {
                var menuString = Regex.Replace(File.ReadAllText(value.Value), @"(.*?\.)(?:md|markdown)", match => match.Groups[1].Value + "html");
                return XmlUtils.WrapAndParse("Menu", new Markdown().Transform(menuString));
            }
            catch (Exception e)
            {
                throw new OptionReaderException(metaData.Lines.First().Number, metaData.Lines.Last().Number, e.Message);
            }
        }
    }
}