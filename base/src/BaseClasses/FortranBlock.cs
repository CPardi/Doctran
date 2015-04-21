//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using Doctran.Fbase.Common;

namespace Doctran.BaseClasses
{
    public abstract class FortranBlock
    {
        public int Weight = 0;
        public bool CheckInternal = true;
        public abstract bool BlockStart(Type parentType, List<FileLine> lines, int lineIndex);
        public abstract bool BlockEnd(Type parentType, List<FileLine> lines, int lineIndex);
        public abstract List<FortranObject> ReturnObject(FortranObject parent, List<FileLine> lines);
    }
}
