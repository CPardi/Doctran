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
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;
    using MarkdownSharp;
    using Options;
    using ParsingElements;
    using ParsingElements.Information;
    using Utilitys;

    internal class MenuOptionAttribute : OptionListAttribute
    {
        public MenuOptionAttribute()
            : base("Menu", typeof(List<XElement>))
        {
        }

        private Markdown MarkdownProcessor => new Markdown();

        public override object InformationToProperty(IInformation information, Type propertyType)
        {
            var value = information as IInformationValue;
            if (value == null)
            {
                throw new OptionReaderException(information.Lines.First().Number, information.Lines.Last().Number, $"'{information.Name}' must be a value type.");
            }

            try
            {
                // Change .md or .markdown extensions to .html and change macros to an XML form.
                var menuString =
                    MarkdownProcessor.Transform(
                        ParsingUtils.ReplaceMacros(
                            Regex.Replace(
                                OtherUtils.ReadAllText(value.Value),
                                @"\w+?\.(?:md|markdown)",
                                match => PathUtils.ChangeExtension(match.Value, "html"))));
                return XmlUtils.WrapAndParse("Menu", menuString);
            }
            catch (Exception e)
            {
                throw new OptionReaderException(information.Lines.First().Number, information.Lines.Last().Number, e.Message);
            }
        }
    }
}