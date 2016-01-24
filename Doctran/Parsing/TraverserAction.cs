// <copyright file="TraverserAction.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing
{
    using System;
    using Helper;

    public class TraverserAction<T> : ITraverserAction<T>
        where T : IFortranObject
    {
        public TraverserAction(Action<T, IErrorListener<TraverserException>> act)
        {
            this.Act = act;
        }

        public Action<T, IErrorListener<TraverserException>> Act { get; }

        public IErrorListener<TraverserException> ErrorListener { get; set; } = new StandardErrorListener<TraverserException>();

        public Type ForType => typeof(T);

        Action<object, IErrorListener<TraverserException>> ITraverserAction.Act => (obj, errLis) => this.Act((T)obj, errLis);
    }
}