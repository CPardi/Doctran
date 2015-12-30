// <copyright file="DynamicErrorListener.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Output.Html
{
    using System.Collections.Generic;
    using javax.xml.transform;
    using net.sf.saxon.lib;

    internal class DynamicErrorListener : UnfailingErrorListener
    {
        public List<TransformerException> Errors { get; set; } = new List<TransformerException>();

        public List<TransformerException> FatalErrors { get; set; } = new List<TransformerException>();

        public List<TransformerException> Warnings { get; set; } = new List<TransformerException>();

        public void error(TransformerException te)
        {
            this.Errors.Add(te);
        }

        public void fatalError(TransformerException te)
        {
            this.FatalErrors.Add(te);
        }

        public void warning(TransformerException te)
        {
            this.Warnings.Add(te);
        }
    }
}