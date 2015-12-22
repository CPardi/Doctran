// <copyright file="HtmlOutputter.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Output
{
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;
    using Saxon.Api;

    public class HtmlOutputter
    {
        // Create xslt variables
        private readonly DocumentBuilder _builder;

        // Contains saxon dll information such as version no.
        private readonly Processor _processor = new Processor();

        // Transforms one Xml to another using Xslt.
        private readonly XsltTransformer _transformer;

        // Transforms one Xml to another using Xslt.
        private readonly XsltTransformer _transformerPreprocess;

        public HtmlOutputter(string xsltPathAndName)
        {
            // Handles the stylesheet compilation.
            var compiler = _processor.NewXsltCompiler();

            // Load a stylesheet at a from text reader, v9.6 doesent like using a Uri for some reason.            
            compiler.BaseUri = new Uri(xsltPathAndName);

            // Create transformer for preprocess.
            TextReader textreaderPreprocess = File.OpenText(xsltPathAndName + "_pre.xslt");
            var executablePreprocess = compiler.Compile(textreaderPreprocess);
            _transformerPreprocess = executablePreprocess.Load();

            // Create transformer for Html output.
            var textreader = File.OpenText(xsltPathAndName + ".xslt");
            var executable = compiler.Compile(textreader);            
            _transformer = executable.Load();

            _builder = _processor.NewDocumentBuilder();
        }

        public void SaveToDisk(XDocument xDocument, string outputDirectory)
        {
            //
            // PRE-PROCESS
            //

            // The transformation destination variable.
            var destinationPreprocess = new DomDestination();

            // Load the source document from an XMLReader.
            var reader = xDocument.CreateReader();

            // Set the root node of the source document to be the initial context node
            _transformerPreprocess.InitialContextNode = _builder.Build(reader);

            // BaseOutputUri is only necessary for xsl:result-document.
            _transformerPreprocess.BaseOutputUri = new Uri(Path.GetFullPath(outputDirectory));

            //Output the transform to the above variable.
            _transformerPreprocess.Run(destinationPreprocess);

            ///
            /// OUTPUT MAIN
            ///

            // The transformation destination variable.
            var destination = new DomDestination();

            // Set the root node of the source document to be the initial context node
            _transformer.InitialContextNode = _builder.Build(destinationPreprocess.XmlDocument);

            // BaseOutputUri is only necessary for xsl:result-document.
            _transformer.BaseOutputUri = new Uri(Path.GetFullPath(outputDirectory));

            // Pass the output directory to the stylesheet.
            this.SetParameter("workingDirectory", _transformerPreprocess.BaseOutputUri.AbsolutePath);

            //Output the transform to the above variable.
            _transformer.Run(destination);
        }

        public void SetParameter(string name, int iNum)
        {
            _transformer.SetParameter(new QName(name), new XdmAtomicValue(iNum));
        }

        public void SetParameter(string name, string str)
        {
            _transformer.SetParameter(new QName(name), new XdmAtomicValue(str));
        }

        public void SetParameter(string name, XmlReader reader)
        {
            _transformer.SetParameter(new QName(name), _builder.Build(reader));
        }
    }
}