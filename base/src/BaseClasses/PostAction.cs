//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Doctran.BaseClasses;

using Doctran.Fbase.Projects;
using Doctran.Fbase.Files;

namespace Doctran.BaseClasses
{
    public abstract class PostAction
    {
        Type T;
        protected PostAction(Type T)
        {
            this.T = T;
        }
        public bool Is(FortranObject Obj)
        {
            return Obj.GetType().IsSubclassOf(T) | Obj.GetType() == T;
        }
        public abstract void PostObject(ref FortranObject obj);
    }
}