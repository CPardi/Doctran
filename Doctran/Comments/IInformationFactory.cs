// <copyright file="IInformationFactory.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Comments
{
    using System.Collections.Generic;
    using Helper;
    using Parsing;

    public interface IInformationFactory
    {
        IEnumerable<IInformation> Create(int depth, string value, IEnumerable<IFortranObject> subObjects, List<FileLine> lines);
    }
}