// <copyright file="TypeSwitch.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Helper
{
    using System;
    using System.Collections.Generic;

    public class TypeSwitch
    {
        private readonly Dictionary<Type, Action<object>> _matches = new Dictionary<Type, Action<object>>();

        public TypeSwitch Case<T>(Action<T> action)
        {
            _matches.Add(typeof(T), x => action((T)x));
            return this;
        }

        public void Switch(object x)
        {
            _matches[x.GetType()](x);
        }
    }
}