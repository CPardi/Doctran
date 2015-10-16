//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using Saxon.Api;

namespace Doctran.Fbase.Outputters
{
    public class HtmlOutputter
    {
        // Transforms one Xml to another using Xslt.
        private readonly XsltTransformer _transformerPreprocess;

        // Transforms one Xml to another using Xslt.
        private readonly XsltTransformer _transformer;

        // Contains saxon dll information such as version no.
        private readonly Processor _processor = new Processor();

        // Create xslt variables
        private readonly DocumentBuilder _builder;

        public HtmlOutputter(string xsltPathAndName)
        {
            // Handles the stylesheet compilation.
            XsltCompiler compiler = _processor.NewXsltCompiler();

            // Load a stylesheet at a from text reader, v9.6 doesent like using a Uri for some reason.            
            compiler.BaseUri = new Uri(xsltPathAndName);

            TextReader textreader = File.OpenText(xsltPathAndName + "_pre.xslt");
            XsltExecutable executable = compiler.Compile(textreader);
                        _transformerPreprocess = executable.Load();
            
            textreader = File.OpenText(xsltPathAndName + ".xslt");
            try
            {
                executable = compiler.Compile(textreader);
            }
            finally
            {                
                foreach (var i in compiler.ErrorList) Console.WriteLine(Environment.NewLine + i.ToString());
            }
            _transformer = executable.Load();

            _builder = _processor.NewDocumentBuilder();
        }

        public void SetParameter(string name, int iNum)
        {
            _transformer.SetParameter(new Saxon.Api.QName(name), new XdmAtomicValue(iNum));
        }

        public void SetParameter(string name, string str)
        {
            _transformer.SetParameter(new Saxon.Api.QName(name), new XdmAtomicValue(str));
        }

        public void SetParameter(string name, XmlReader reader)
        {
            _transformer.SetParameter(new Saxon.Api.QName(name), _builder.Build(reader));
        }

        public void SaveToDisk(XDocument xDocument, string outputDirectory)
        {
            ///
            /// PRE-PROCESS
            ///
            // The transformation destination variable.
            DomDestination destinationPreprocess = new DomDestination();

            // Load the source document from an XMLReader.
            XmlReader reader = xDocument.CreateReader();
            
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
            DomDestination destination = new DomDestination();

            // Set the root node of the source document to be the initial context node
            _transformer.InitialContextNode = _builder.Build(destinationPreprocess.XmlDocument);

            // BaseOutputUri is only necessary for xsl:result-document.
            _transformer.BaseOutputUri = new Uri(Path.GetFullPath(outputDirectory));

            // Pass the output directory to the stylesheet.
            this.SetParameter("workingDirectory", _transformerPreprocess.BaseOutputUri.AbsolutePath);

            //Output the transform to the above variable.
            _transformer.Run(destination);
        }


    }
}
