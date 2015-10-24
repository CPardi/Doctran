﻿//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Doctran.Parsing
{
    using System.Collections.Generic;
    using Helper;

    public abstract class FortranBlock
    {
        protected FortranBlock(string name, bool check_internal = true, bool explicit_end = true, int weight = 0)
        {
            this.Name = name;
            this.CheckInternal = check_internal;
            this.ExplicitEnd = explicit_end;
            this.Weight = weight;
        }

        public bool CheckInternal { get; private set; }
        public bool ExplicitEnd { get; private set; }
        public int Weight { get; private set; }
        public string Name { get; private set; }        

        public abstract bool BlockStart(string parentBlockName, List<FileLine> lines, int lineIndex);
        public abstract bool BlockEnd(string parentBlockName, List<FileLine> lines, int lineIndex);
        public abstract IEnumerable<FortranObject> ReturnObject(IEnumerable<FortranObject> subObjects, List<FileLine> lines);
    }
}