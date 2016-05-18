// <copyright file="IInterfaceXElements.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.XmlSerialization
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;
    using Parsing;

    public interface IInterfaceXElements
    {
        Type ForType { get; }

        bool ShouldCreate(object from);

        IEnumerable<XObject> Create(object from);
    }

    public interface IInterfaceXElements<in TParsed>
        where TParsed : IFortranObject
    {
        Type ForType { get; }

        bool ShouldCreate(TParsed from);

        IEnumerable<XObject> Create(TParsed from);
    }
}