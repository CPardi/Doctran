//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

using Doctran.Fbase.Common;
using Doctran.Fbase.Comments;

namespace Doctran.BaseClasses
{
    public interface IPlugin
    {
        void Initialize();
        void WriteInformation();
    }
}

