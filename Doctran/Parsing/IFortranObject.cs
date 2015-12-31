// <copyright file="IFortranObject.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing
{
    using System.Collections.Generic;
    using Helper;

    public interface IFortranObject
    {
        List<FileLine> Lines { get; }

        //IFortranObject Parent { get; set; }

        //List<IFortranObject> SubObjects { get; }

        //void AddSubObject(IFortranObject obj);

        //void AddSubObjects(IEnumerable<IFortranObject> objs);

        //T GoUpTillType<T>() where T : IFortranObject;
    }
}