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
    using System.Xml.Linq;
    using Comments;
    using MarkdownSharp;
    using OptionsReaderCore;
    using Parsing;
    using Utilitys;

    internal class UserPageOptionAttribute : OptionListAttribute
    {
        public UserPageOptionAttribute()
            : base("UserPage", typeof(List<XElement>))
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
                var path = value.Value;
                var xElement = new XElement("UserPage");
                xElement.Add(new XElement("Path", PathUtils.ChangeExtension(path, ".html")));
                xElement.Add(XmlUtils.WrapAndParse("Content", new Markdown().Transform(File.ReadAllText(path))));
                return xElement;
            }
            catch (Exception e)
            {
                throw new OptionReaderException(metaData.Lines.First().Number, metaData.Lines.Last().Number, e.Message);
            }
        }
    }
}