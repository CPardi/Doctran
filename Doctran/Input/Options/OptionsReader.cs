// <copyright file="OptionsReader.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Input.Options
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Helper;
    using Parsing;
    using ParsingElements;
    using ParsingElements.FortranBlocks;
    using Utilitys;

    /// <summary>
    ///     Parses text and populates an options class with the parsed values. Each property of the options class corresponds
    ///     to a single <see cref="IInformation"/>, identified by propertyType typeName.
    /// </summary>
    /// <typeparam name="TOptions">The generic class representing the options to be passed to the parser.</typeparam>
    public class OptionsReader<TOptions>
    {
        private readonly Parser _parser;

        /// <summary>
        ///     Holds the intermediary instances of <see cref="IInformation" /> that are generated while parsing.
        /// </summary>
        private List<IInformation> _informationList;

        /// <summary>
        ///     Initializes a new instance of the <see cref="OptionsReader{TOptions}" /> class.
        /// </summary>
        /// <param name="maxDepth">The maximun depth of the option groups to be searched for.</param>
        /// <param name="name">The parser's name to be used within any exceptions.</param>
        public OptionsReader(int maxDepth, string name)
        {
            this.Name = name;

            _parser = new Parser("DoctranOptions", InformationBlock.MultiDepthEnumeration(1, maxDepth))
            {
                ErrorListener = new ErrorListener<ParserException>(
                    warn => this.ErrorListener.Warning(new OptionReaderException(warn.StartLine, warn.EndLine, warn.Message)),
                    err => this.ErrorListener.Error(new OptionReaderException(err.StartLine, err.EndLine, err.Message)))
            };
        }

        /// <summary>
        ///     Gets or sets the error listener be used for reporting errors.
        /// </summary>
        public IErrorListener<OptionReaderException> ErrorListener { get; set; } = new StandardErrorListener<OptionReaderException>();

        /// <summary>
        ///     Gets the name of the parser. This name will be used within exceptions.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Parses the lines of text, supplied from the constructor.
        /// </summary>
        /// <param name="options">The instance of <see cref="TOptions" /> to be populated.</param>
        /// <param name="sourceName">The name of the source. If from a file use file path.</param>
        /// <param name="source">The lines of text to be parsed.</param>
        /// <exception cref="ParserException">Thrown when the text being parsed contains an exception.</exception>
        /// <exception cref="InvalidPropertyTypeException">
        ///     Throw when the developer has applied an attribute to a property of an
        ///     incorrect type.
        /// </exception>
        public void Parse(TOptions options, string sourceName, string source)
        {
            _informationList = _parser.Parse(sourceName, source, this.Preprocessor).SubObjects.Cast<IInformation>().ToList();

            // If we find a property with the default option attribute then it will be stored here.
            Tuple<PropertyInfo, IDefaultOptionAttribute> defaultInfo = null;

            var analysedOptions = new List<string>();

            // Loop through all the properties of the options class.
            foreach (var prop in typeof(TOptions).GetProperties())
            {
                this.GetDefaultOption(prop, ref defaultInfo);

                // Get any option Attributes that belong to the current property.
                var propOptions = prop.GetCustomAttributes(typeof(IOptionAttribute), true).OfType<IOptionAttribute>().ToList();

                CheckMultiOptions(propOptions);

                foreach (var option in propOptions)
                {
                    if (analysedOptions.Contains(option.Name))
                    {
                        throw new InvalidAttributesException($"The attribute for options of name '{option.Name}' was applied twice.");
                    }

                    analysedOptions.Add(option.Name);

                    var scalarOption = option as OptionAttribute;
                    var optionList = option as IOptionListAttribute;

                    // If there is no option defined or the option doesn't appear within the text, then skip.
                    List<IInformation> informationOfName;
                    if (!(informationOfName = this.GetInformationOfName(option.Name)).Any())
                    {
                        if (optionList != null && optionList.InitializeAsDefault)
                        {
                            prop.SetValue(options, (IEnumerable)Activator.CreateInstance(optionList.InitializationType), null);
                        }
                        else if (scalarOption?.DefaultValue != null)
                        {
                            prop.SetValue(options, scalarOption.DefaultValue, null);
                        }

                        continue;
                    }

                    // If this property is a enumeration, then treat each option as a list item.
                    if (optionList != null)
                    {
                        if (!IsGenericEnumerable(prop.PropertyType))
                        {
                            throw new InvalidPropertyTypeException("DefaultOptionAttribute", typeof(IEnumerable<>), prop.PropertyType);
                        }

                        if (optionList.ListMode == ListMode.SetValue)
                        {
                            this.SetValueList(option.InformationToProperty, options, informationOfName, prop, optionList.InitializationType);
                        }
                        else if (optionList.ListMode == ListMode.AddTo)
                        {
                            this.AddToList(option.InformationToProperty, options, informationOfName, prop, optionList.InitializationType);
                        }
                    }
                    else
                    {
                        this.AssignScalar(option.InformationToProperty, options, informationOfName, prop, prop.PropertyType);
                    }
                }
            }

            // If a default is specified, then assign any remaining values to it.
            if (defaultInfo != null && _informationList.Any())
            {
                this.AddToList(defaultInfo.Item2.InformationToProperty, options, _informationList, defaultInfo.Item1, defaultInfo.Item2.InitializationType);
            }
        }

        private static void CheckMultiOptions(IEnumerable<IOptionAttribute> propOptions)
        {
            var optionAttributes = propOptions.OfType<IOptionListAttribute>().ToList();
            var optTypePrev = optionAttributes.FirstOrDefault()?.InitializationType;

            foreach (var optType in optionAttributes.Skip(1).Select(opt => opt.InitializationType))
            {
                if (optType != optTypePrev)
                {
                    throw new InvalidAttributesException($"Initialization types of attributes of the same property are not the same, '{optType}' and '{optTypePrev}'.");
                }

                optTypePrev = optType;
            }
        }

        /// <summary>
        ///     Returns whether the type <see param="t" /> or its inherited types are of <see cref="IEnumerable{T}" />.
        /// </summary>
        /// <param name="t">The type to be checked.</param>
        /// <returns>True if <see param="t" /> is a <see cref="IEnumerable{T}" /> and false otherwise.</returns>
        private static bool IsGenericEnumerable(Type t)
        {
            var genArgs = t.GetGenericArguments();
            if (genArgs.Length == 1 &&
                typeof(IEnumerable<>).MakeGenericType(genArgs).IsAssignableFrom(t))
            {
                return true;
            }

            return t.BaseType != null && IsGenericEnumerable(t.BaseType);
        }

        /// <summary>
        ///     The string to be used within uniquenes exception messages.
        /// </summary>
        /// <param name="optionName">The name of the option that should be unique.</param>
        /// <returns>The exception message</returns>
        private static string UniquenessExceptionString(string optionName)
            => $"The option '{optionName}' must be unique.";

        /// <summary>
        ///     Assigns and converts the <paramref name="valuesOfName" /> meta datas to <paramref name="propertyInfo" /> within
        ///     <paramref name="options" />.
        /// </summary>
        /// <param name="convert">The delegate to convert meta-data into the property type.</param>
        /// <param name="options">The instance of <see cref="TOptions" /> to be populated.</param>
        /// <param name="valuesOfName">The meta-data acquired from parsing the text.</param>
        /// <param name="propertyInfo">The <see cref="PropertyInfo" /> of the property to be assigned.</param>
        /// <param name="initializationType">The type to initialize the property to.</param>
        /// <exception cref="ParserException">The exception is throw when there has been an exception adding an object to the list.</exception>
        private void AddToList(Func<IInformation, Type, object> convert, TOptions options, IEnumerable<IInformation> valuesOfName, PropertyInfo propertyInfo, Type initializationType)
        {
            // If the property has not been initialized, then do it.
            if (propertyInfo.GetValue(options, null) == null)
            {
                propertyInfo.SetValue(options, (IEnumerable)Activator.CreateInstance(initializationType), null);
            }

            // Get the property list.
            var propList = (IList)propertyInfo.GetValue(options, null);

            // Get the type of generic parameter to initialize to, within the enumeration.
            var initTo = initializationType.GetInterface(typeof(IEnumerable<>).Name).GetGenericArguments()[0];

            // Add the text options to it.
            foreach (var v in valuesOfName)
            {
                var c = this.CheckAndConvertValue(convert, v, initTo);
                if (c == null)
                {
                    continue;
                }

                try
                {
                    propList.Add(c);
                }
                catch (Exception e)
                {
                    this.ErrorListener.Error(new OptionReaderException(v.Lines.First().Number, v.Lines.Last().Number, e.Message));
                }
            }
        }

        private void SetValueList(Func<IInformation, Type, object> convert, TOptions options, IEnumerable<IInformation> valuesOfName, PropertyInfo propertyInfo, Type initializationType)
        {
            // Create an instance of the list specified.
            var list = (IList)Activator.CreateInstance(initializationType);

            // The type parameter to convert string values to.
            var initTo = initializationType.GetInterface(typeof(IEnumerable<>).Name).GetGenericArguments()[0];

            // Add the text options to it.
            foreach (var v in valuesOfName)
            {
                var c = this.CheckAndConvertValue(convert, v, initTo);
                if (c == null)
                {
                    continue;
                }

                try
                {
                    list.Add(c);
                }
                catch (Exception e)
                {
                    this.ErrorListener.Error(new OptionReaderException(v.Lines.First().Number, v.Lines.Last().Number, e.Message));
                }
            }

            // Set the value of the list property
            propertyInfo.SetValue(options, list, null);
        }

        /// <summary>
        ///     Assigns and converts the <paramref name="valuesOfName" /> meta-data to <paramref name="propertyInfo" /> within
        ///     <paramref name="options" />.
        /// </summary>
        /// <param name="convert">The delegate to convert meta-data into the property type.</param>
        /// <param name="options">The instance of <see cref="TOptions" /> to be populated.</param>
        /// <param name="valuesOfName">The meta-data acquired from parsing the text.</param>
        /// <param name="propertyInfo">The <see cref="PropertyInfo" /> of the property to be assigned.</param>
        /// <param name="initializationType">The type to initialize the property to.</param>
        /// <exception cref="ParserException">The exception is throw when the meta-data is not unique.</exception>
        private void AssignScalar(Func<IInformation, Type, object> convert, TOptions options, ICollection<IInformation> valuesOfName, PropertyInfo propertyInfo, Type initializationType)
        {
            if (!valuesOfName.Any())
            {
                return;
            }

            if (valuesOfName.Count > 1)
            {
                foreach (var md in valuesOfName)
                {
                    this.ErrorListener.Error(new OptionReaderException(md.Lines.First().Number, md.Lines.Last().Number, UniquenessExceptionString(md.Name)));
                }

                return;
            }

            var c = this.CheckAndConvertValue(convert, valuesOfName.Single(), initializationType);

            if (c != null)
            {
                propertyInfo.SetValue(options, c, null);
            }
        }

        /// <summary>
        ///     Converts the data to the type <paramref name="type" />.
        /// </summary>
        /// <param name="convert">The conversion procedure.</param>
        /// <param name="information">The information to convert.</param>
        /// <param name="type">The type to convert the meta-data to.</param>
        /// <returns>The converted meta-data.</returns>
        /// <exception cref="ParserException">
        ///     This is thrown when there has been an exception converting <paramref name="information" />
        ///     to type <paramref name="type" />.
        /// </exception>
        private object CheckAndConvertValue(Func<IInformation, Type, object> convert, IInformation information, Type type)
        {
            try
            {
                return convert(information, type);
            }
            catch (OptionReaderException e)
            {
                this.ErrorListener.Error(e);
                return null;
            }
        }

        private void GetDefaultOption(PropertyInfo prop, ref Tuple<PropertyInfo, IDefaultOptionAttribute> defaultInfo)
        {
            // Get the default option, if any
            var defaultOption = prop.GetCustomAttributes(typeof(DefaultOptionAttribute), true).SingleOrDefault()
                as IDefaultOptionAttribute;

            if (defaultOption == null)
            {
                return;
            }

            if (!IsGenericEnumerable(prop.PropertyType))
            {
                throw new InvalidPropertyTypeException(nameof(defaultOption), typeof(IEnumerable<>), prop.PropertyType);
            }

            defaultInfo = new Tuple<PropertyInfo, IDefaultOptionAttribute>(prop, defaultOption);
        }

        /// <summary>
        ///     Returns the meta-data that textes <see ref="typeName" /> and removes it from <see cref="_informationList" />.
        /// </summary>
        /// <param name="typeName">The propertyType typeName of the meta-data to be return.</param>
        /// <returns>A list of meta-data values or groups of propertyType typeName <see ref="typeName" /></returns>
        private List<IInformation> GetInformationOfName(string typeName)
        {
            var valuesOfName = _informationList
                .Where(md => md.Name == typeName).ToList();

            foreach (var v in valuesOfName)
            {
                _informationList.Remove(v);
            }

            return valuesOfName;
        }

        private IEnumerable<FileLine> Preprocessor(string source)
        {
            var lines = StringUtils.ConvertToFileLineList(source);
            return (from line in lines
                select new FileLine(line.Number, line.Text != string.Empty ? "!>" + line.Text : string.Empty)).ToList();
        }
    }
}