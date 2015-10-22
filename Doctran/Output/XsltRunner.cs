//-----------------------------------------------------------------------
// <copyright file="XsltRunner.cs" company="Christopher Pardi">
// Copyright © 2015 Christopher Pardi
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>
//-----------------------------------------------------------------------

namespace Doctran.Output
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;
    using javax.xml.transform;
    using Reporting;
    using Saxon.Api;

    internal class XsltRunner
    {
        // Create xslt variables
        private readonly DocumentBuilder _builder;

        // Transforms one Xml to another using Xslt.
        private readonly XsltExecutable _executable;

        // Contains saxon dll information such as version no.
        private readonly Processor _processor = new Processor();

        public XsltRunner(string stylesheetPath)
        {
            StylesheetPath = stylesheetPath;

            // Save a builder for later.
            _builder = _processor.NewDocumentBuilder();

            // Handles the stylesheet compilation.
            var compiler = _processor.NewXsltCompiler();

            // Load a stylesheet at a from text reader, v9.6 doesent like using a Uri for some reason.
            compiler.BaseUri = new Uri(Path.GetFullPath(StylesheetPath));

            TextReader textreader = File.OpenText(stylesheetPath);

            var errorListener = new TransformerErrorListener();
            try
            {
                compiler.GetUnderlyingCompilerInfo().setErrorListener(errorListener);
                _executable = compiler.Compile(textreader);
            }
            catch (TransformerException)
            {
                ReportCompilerErrors(errorListener);
            }
        }

        public string StylesheetPath { get; }

        public XmlDocument Run(XNode xDocument, string baseOutputUri = null, params KeyValuePair<string, object>[] arguments)
        {
            // Load the source document from an XMLReader.
            var reader = xDocument.CreateReader();

            // Load execuable.
            var transformer = _executable.Load();

            // Set the root node of the source document to be the initial context node
            transformer.InitialContextNode = _builder.Build(reader);

            // Set where to output xsl:result-document constructs.
            if (baseOutputUri != null)
            {
                transformer.BaseOutputUri = new Uri(baseOutputUri);
            }

            // Set parameters.
            this.SetParams(ref transformer, arguments);

            // The transformation destination variable.
            var destination = new DomDestination();

            // Errors will be reported by the exception handling.
            transformer.Implementation.setErrorListener(new TransformerErrorListener());

            // Output the transform to the above variable.
            var listener = new DynamicErrorListener();
            try
            {
                transformer.Implementation.setErrorListener(listener);
                transformer.Run(destination);
            }
            catch (DynamicError)
            {
                ReportCompilerErrors(listener);
            }

            // Clear any set parameters.
            transformer.Reset();

            return destination.XmlDocument;
        }

        private void ReportCompilerErrors(TransformerErrorListener listener)
        {
            // Need to hold exception within another because TransformerException is a base type.
            Report.Warnings(
                (publisher, warning) =>
                {
                    publisher.AddWarningDescription("XSLT compilation warning");
                    publisher.AddReason(warning.getCause().Message);
                    publisher.AddLocation($"At line '{warning.getLocator().getLineNumber()}', column '{warning.getLocator().getColumnNumber()}' of '{StylesheetPath}'.");
                },
                listener.Warnings);

            Report.Warnings(
                (publisher, error) =>
                {
                    publisher.AddWarningDescription("Non-fatal XSLT stylesheet error.");
                    publisher.AddReason(error.getCause().Message);
                    publisher.AddLocation($"At line '{error.getLocator().getLineNumber()}', column '{error.getLocator().getColumnNumber()}' of '{StylesheetPath}'.");
                },
                listener.Errors);

            Report.Errors(
                (publisher, fatalError) =>
                {
                    publisher.AddErrorDescription("Fatal XSLT stylesheet error.");
                    publisher.AddReason(fatalError.getMessage());
                    publisher.AddLocation($"At line '{fatalError.getLocator().getLineNumber()}', column '{fatalError.getLocator().getColumnNumber()}' of '{fatalError.getLocator().getSystemId()}'.");
                },
                listener.FatalErrors);
        }

        private void ReportCompilerErrors(DynamicErrorListener listener)
        {
            // Need to hold exception within another because TransformerException is a base type.
            Report.Warnings(
                (publisher, warning) =>
                {
                    var sourceLocator = warning.getLocator();
                    publisher.AddWarningDescription("XSLT runtime warning");
                    publisher.AddReason(warning.Message);
                    publisher.AddLocation(
                        sourceLocator == null
                            ? $"In '{StylesheetPath}'."
                            : $"At line '{sourceLocator.getLineNumber()}', column '{sourceLocator.getColumnNumber()}' of '{sourceLocator.getSystemId()}'.");
                },
                listener.Warnings);

            Report.Warnings(
                (publisher, error) =>
                {
                    var sourceLocator = error.getLocator();
                    publisher.AddWarningDescription("Non-fatal XSLT runtime error.");
                    publisher.AddReason(error.Message);
                    publisher.AddLocation(
                        sourceLocator == null
                            ? $"In '{StylesheetPath}'."
                            : $"At line '{sourceLocator.getLineNumber()}', column '{sourceLocator.getColumnNumber()}' of '{sourceLocator.getSystemId()}'.");
                },
                listener.Errors);

            Report.Errors(
                (publisher, fatalError) =>
                {
                    var sourceLocator = fatalError.getLocator();
                    publisher.AddErrorDescription("Fatal XSLT runtime error.");
                    publisher.AddReason(fatalError.Message);
                    publisher.AddLocation(
                        sourceLocator == null
                            ? $"In '{StylesheetPath}'."
                            : $"At line '{sourceLocator.getLineNumber()}', column '{sourceLocator.getColumnNumber()}' of '{sourceLocator.getSystemId()}'.");
                },
                listener.FatalErrors);
        }

        private void SetParams(ref XsltTransformer transformer, KeyValuePair<string, object>[] arguments)
        {
            foreach (var arg in arguments)
            {
                var xmlArg = arg.Value as XmlReader;
                if (xmlArg != null)
                {
                    transformer.SetParameter(new QName(arg.Key), _builder.Build(xmlArg));
                    continue;
                }

                if (arg.Value is int)
                {
                    transformer.SetParameter(new QName(arg.Key), new XdmAtomicValue((int)arg.Value));
                    continue;
                }

                var sArg = arg.Value as string;
                if (sArg != null)
                {
                    transformer.SetParameter(new QName(arg.Key), new XdmAtomicValue(sArg));
                    continue;
                }

                // If we get here then the parameter is unrecognized, so let the developer know.
                throw new NotImplementedException($"In {nameof(arguments)}, the conversion for the type '{arg.Value.GetType().Name}' of the value from '{arg.Key}' has not yet been implemented.");
            }
        }
    }
}