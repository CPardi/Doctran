// <copyright file="UserPageOptionAttribute.cs" company="Christopher Pardi">
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
    using System.Xml.Linq;
    using Helper;
    using MarkdownSharp;
    using Options;
    using ParsingElements;
    using ParsingElements.Information;
    using Utilitys;

    internal class UserPageOptionAttribute : OptionListAttribute
    {
        public UserPageOptionAttribute()
            : base("UserPage", typeof(List<XElement>))
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
                var xElements = new List<XElement>();

                // The path could contain wildcards, so use a PathList.
                var pathList = new PathList() { PathStorage = PathStorageMode.Relative };
                pathList.Add(value.Value.Trim());

                foreach (var path in pathList)
                {
                    var xElement = new XElement("UserPage");
                    xElement.Add(new XElement("Path", PathUtils.ChangeExtension(path, ".html")));
                    xElement.Add(XmlUtils.WrapAndParse("Content", new Markdown().Transform(OtherUtils.ReadAllText(path))));
                    xElements.Add(xElement);
                }

                return xElements;
            }
            catch (Exception e)
            {
                throw new OptionReaderException(information.Lines.First().Number, information.Lines.Last().Number, e.Message);
            }
        }
    }
}