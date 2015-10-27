//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Doctran.Parsing
{
    using System;

    public class TraverserAction<T> : ITraverserAction<T>
        where T : FortranObject
    {
        public TraverserAction(Action<T> act)
        {
            this.Act = act;
        }

        public Type ForType => typeof(T);

        public Action<T> Act { get; }

        Action<object> ITraverserAction.Act => obj => this.Act((T) obj);
    }
}