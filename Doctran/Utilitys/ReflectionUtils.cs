// <copyright file="ReflectionUtils.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Utilitys
{
    using System;
    using System.Linq;
    using System.Reflection;

    public static class ReflectionUtils
    {
        public static T GetAssemblyAttribute<T>(this Assembly ass)
            where T : Attribute
        {
            var attributes = ass.GetCustomAttributes(typeof(T), false);
            return
                attributes.Length != 0
                    ? attributes.OfType<T>().Single()
                    : null;
        }

        public static void ForTypeAndInterfaces(this Type @this, Action<Type> action)
        {
            action(@this);
            foreach (var inter in @this.GetInterfaces())
            {
                action(inter);
            }
        }
    }
}