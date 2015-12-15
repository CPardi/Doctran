// <copyright file="ValueAttribute.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Input.OptionFile
{
    using System;

    public class ValueAttribute : Attribute
    {
        public ValueAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }
    }
}