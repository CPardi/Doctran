// <copyright file="IOptionAttribute.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Input.Options
{
    using System;
    using ParsingElements;

    public interface IOptionAttribute
    {
        string Name { get; }

        object InformationToProperty(IInformation information, Type propertyType);
    }
}