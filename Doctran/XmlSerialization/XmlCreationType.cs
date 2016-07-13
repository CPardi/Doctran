// <copyright file="XmlCreationType.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.XmlSerialization
{
    public enum XmlCreationType
    {
        /// <summary>
        /// Specifies that all XML information should be outputted, including from the object, interfaces and sub-objects.
        /// </summary>
        All,

        /// <summary>
        /// Specifies that only the XML value for the object should be outputted. This means information from interfaces and sub-objects will be omitted.
        /// </summary>
        ObjectOnly
    }
}