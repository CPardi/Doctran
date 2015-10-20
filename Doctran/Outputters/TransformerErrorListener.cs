//-----------------------------------------------------------------------
// <copyright file="ErrorMuteListener.cs" company="Christopher Pardi">
// Copyright © 2015 Christopher Pardi
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>
//-----------------------------------------------------------------------

namespace Doctran.Output.XsltRunnerCore
{
    using System.Collections.Generic;
    using javax.xml.transform;

    internal class TransformerErrorListener : ErrorListener
    {
        public List<TransformerException> Errors { get; set; } = new List<TransformerException>();
        public List<TransformerException> FatalErrors { get; set; } = new List<TransformerException>();
        public List<TransformerException> Warnings { get; set; } = new List<TransformerException>();

        public void error(TransformerException te)
        {
            Errors.Add(te);
        }

        public void fatalError(TransformerException te)
        {
            FatalErrors.Add(te);
        }

        public void warning(TransformerException te)
        {
            Warnings.Add(te);
        }
    }
}