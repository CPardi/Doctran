// <copyright file="Parser.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Input.OptionFile
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Comments;
    using Helper;
    using Parsing;
    using Parsing.BuiltIn.FortranBlocks;
    using Parsing.BuiltIn.FortranObjects;
    using Plugins;
    using Reporting;

    public class Parser<TOptions>
    {
        private readonly IDictionary<string, IInformationFactory> _factories = new Dictionary<string, IInformationFactory>();

        public static Parser<TOptions> Default => new Parser<TOptions>();

        public void AddRecognisedOption(string name)
        {
            _factories.Add(name, new OptionFactory(name));
        }

        public void AddRecognisedOption(string name, IInformationFactory factory)
        {
            _factories.Add(name, factory);
        }

        public bool ParseFile(string filePath, TOptions options)
        {
            // Read the project file if there is one.            
            var infos = ParseLines(filePath);

            var infoArray = infos as IInformation[] ?? infos.ToArray();
            var xmlPassThrough =
                from info in infoArray
                where info is XInformation
                let infoXml = info as XInformation
                select infoXml.XEle();

            var props = options.GetType().GetProperties();

            var currentDirectory = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Path.GetFullPath(filePath)) ?? ".");
            foreach (var prop in props)
            {
                var xmlAttr = prop.GetCustomAttributes(typeof(XmlPassThroughOptionsAttribute), true).SingleOrDefault() as XmlPassThroughOptionsAttribute;
                if (xmlAttr != null)
                {
                    prop.SetValue(options, new XElement(xmlAttr.RootName, xmlPassThrough), null);
                }
                var valueAttr = prop.GetCustomAttributes(typeof(ValueAttribute), true).SingleOrDefault() as ValueAttribute;
                if (valueAttr != null)
                {
                    var list = prop.GetValue(options, null) as ICollection<string>;
                    var infoName = infoArray.Where(i => i is Option && i.Name == valueAttr.Name).Select(i => (i as Option).Value);
                    if (list == null)
                    {
                        prop.SetValue(options, infoName.SingleOrDefault(), null);
                    }
                    else
                    {
                        try
                        {
                            foreach (var str in infoName)
                            {
                                list.Add(str);
                            }
                        }
                        catch (Exception e)
                        {
                            Report.Error(
                                (pub, ex) =>
                                {
                                    pub.AddErrorDescription("Error within project file.");
                                    pub.AddReason(e.Message);
                                }, e);
                        }
                    }
                }
            }
            Directory.SetCurrentDirectory(currentDirectory);
            return true;
        }

        private IEnumerable<IInformation> ParseLines(string path)
        {
            var fileName = Path.GetFileName(path);

            var currentDirectory = Directory.GetCurrentDirectory();

            try
            {
                Directory.SetCurrentDirectory(Path.GetDirectoryName(Path.GetFullPath(path)));
            }
            catch (IOException e)
            {
                Report.Error(
                    (pub, ex) =>
                    {
                        pub.AddErrorDescription("Could not locate project file.");
                        pub.AddReason(e.Message);
                    }, e);
                throw;
            }

            if (string.IsNullOrEmpty(path))
            {
                return new List<IInformation>();
            }

            var info = new InformationBlock(1, _factories);

            var infoList = new List<InformationBlock>();
            infoList.Add(info);
            infoList.AddRange(from i in Enumerable.Range(2, 4)
                select new InformationBlock(i));

            var parser = new Parser(infoList, new Preprocessor());
            try
            {
                var result = parser.ParseLines(this.ReadAndPreProcessFile(fileName)).SubObjects;
                TestParseResults(result);
                var resultInfo = result.Cast<IInformation>();

                Directory.SetCurrentDirectory(currentDirectory);

                return resultInfo;
            }
            catch (IOException e)
            {
                Report.Error(
                    (pub, ex) =>
                    {
                        pub.AddErrorDescription("Error within project file.");
                        pub.AddReason(e.Message);
                    }, e);
                throw;
            }
        }

        private List<FileLine> ReadAndPreProcessFile(string path)
        {
            return (from line in SourceFile.ReadFile(path)
                select new FileLine(line.Number, line.Text != "" ? "!>" + line.Text : "")
                ).ToList();
        }

        private void TestDepth(IEnumerable<IFortranObject> infos, int depth = 1)
        {
            foreach (var i in infos)
            {
                if ((i as IInformation).Depth != depth)
                {
                    throw new WrongDepthException(i.Lines.First(), depth, (i as IInformation).Depth);
                }
                TestDepth(i.SubObjects, depth + 1);
            }
        }

        private void TestParseResults(IEnumerable<IFortranObject> infos)
        {
            TestDepth(infos);
        }
    }
}