//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using Saxon.Api;

namespace Doctran.Fbase.Outputters
{
    public class HtmlOutputter
    {
        private readonly XsltTransformer transformer;
        // Define a processor, creates things that do 'proper' stuff. Also contain saxon dll information such as version no.
        private readonly Processor processor = new Processor();
        private readonly DocumentBuilder builder;

        public HtmlOutputter(String xsltPathAndName)
        {
            // Handles the stylesheet compilation.
            XsltCompiler compiler = processor.NewXsltCompiler();

            // Load a stylesheet at a from text reader, v9.6 doesent like using a Uri for some reason.
            TextReader textreader = File.OpenText(xsltPathAndName);
            compiler.BaseUri = new Uri(xsltPathAndName);
            XsltExecutable executable = compiler.Compile(textreader);
            this.transformer = executable.Load();

            builder = processor.NewDocumentBuilder();
        }

        public void SetParameter(String name, int iNum)
        {
            transformer.SetParameter(new Saxon.Api.QName(name), new XdmAtomicValue(iNum));
        }

        public void SetParameter(String name, XmlReader reader)
        {
            transformer.SetParameter(new Saxon.Api.QName(name), builder.Build(reader));
        }

		public void SaveToDisk(XDocument xDocument, String outputDirectory)
        {
            // The transformation destination variable.
            DomDestination destination = new DomDestination();

            // Load the source document from an XMLReader.
            XmlReader Reader = xDocument.CreateReader();
            
            // Set the root node of the source document to be the initial context node
            transformer.InitialContextNode = processor.NewDocumentBuilder().Build(Reader);

            // BaseOutputUri is only necessary for xsl:result-document.
            transformer.BaseOutputUri = new Uri(Path.GetFullPath(outputDirectory));

            //Output the transform to the above variable.
            transformer.Run(destination);
        }
    }
}
