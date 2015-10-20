//-----------------------------------------------------------------------
// <copyright file="XsltCompilerException.cs" company="Christopher Pardi">
// Copyright © 2015 Christopher Pardi
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>
//-----------------------------------------------------------------------

namespace Doctran.Output
{
    using System;
    using javax.xml.transform;

    internal class XsltCompilerException : ApplicationException
    {
        public XsltCompilerException(TransformerException exception)
        {
            this.TransformerException = exception;
        }

        public TransformerException TransformerException { get; }
    }
}