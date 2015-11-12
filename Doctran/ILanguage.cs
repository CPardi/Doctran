//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.



namespace Doctran
{
    using System.Collections.Generic;
    using Output;
    using Parsing;

    public interface ILanguage
    {
        IEnumerable<FortranBlock> BlocksParsers { get; }

        IEnumerable<Traverser> Traversers { get; }

        IEnumerable<ObjectGroup> ObjectGroups { get; }

        XmlGenerator XmlGenerator { get; }
    }
}

