﻿// <copyright file="IInterfaceXElements.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    public interface IInterfaceXElements
    {
        Type ForType { get; }

        IEnumerable<XElement> Create(object from);
    }

    public interface IInterfaceXElements<in TParsed>
    {
        Type ForType { get; }

        IEnumerable<XElement> Create(TParsed from);
    }
}