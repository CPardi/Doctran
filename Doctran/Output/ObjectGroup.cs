// <copyright file="ObjectGroup.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Output
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;
    using Parsing;

    public abstract class ObjectGroup
    {
        private readonly Type _;

        protected ObjectGroup(Type T)
        {
            this._ = T;
        }

        public bool Is(FortranObject obj)
        {
            return obj.GetType().IsSubclassOf(_) | obj.GetType() == _;
        }

        public XElement XEle(XElement content)
        {
            return this.XEle(new List<XElement> { content });
        }

        public virtual XElement XEle(IEnumerable<XElement> content)
        {
            return null;
        }
    }
}