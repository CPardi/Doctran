// <copyright file="ReflectionUtils.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Utilitys
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class ReflectionUtils
    {
        public static void ForTypeAndInterfaces(this Type @this, Action<Type> action)
        {
            action(@this);
            foreach (var inter in @this.GetInterfaces())
            {
                action(inter);
            }
        }

        public static void ForTypeBaseTypesAndInterfaces(this Type @this, Action<Type> action)
        {
            foreach (var baseType in @this.GetTypeAndBaseTypes())
            {
                action(baseType);
            }

            foreach (var inter in @this.GetInterfaces())
            {
                action(inter);
            }
        }

        public static T GetAssemblyAttribute<T>(this Assembly ass)
            where T : Attribute
        {
            var attributes = ass.GetCustomAttributes(typeof(T), false);
            return
                attributes.Length != 0
                    ? attributes.OfType<T>().Single()
                    : null;
        }

        public static IEnumerable<Type> GetBaseTypes(this Type @this)
        {
            var current = @this;
            while (current != null)
            {
                current = current.BaseType;
                yield return current;
            }
        }

        /// <summary>
        ///     Returns the root type that is not object. If <paramref name="this" /> is <see cref="object" /> or a direct
        ///     extension of <see cref="object" />, then <paramref name="this" /> is returned.
        /// </summary>
        /// <param name="this">The type whose root type is to be found.</param>
        /// <returns>The root type.</returns>
        public static Type GetRootType(this Type @this)
        {
            var current = @this;
            while (current.BaseType != null && current.BaseType != typeof(object))
            {
                current = current.BaseType;
            }

            return current;
        }

        public static IEnumerable<Type> GetTypeAndBaseTypes(this Type @this)
        {
            var current = @this;
            while (current != null)
            {
                yield return current;
                current = current.BaseType;
            }
        }

        public static IEnumerable<Type> GetTypeAndBaseTypesAndInterfaces(this Type @this)
        {
            var typeList = new List<Type>();
            var current = @this;
            while (current != null)
            {
                typeList.Add(current);
                current = current.BaseType;
            }

            typeList.AddRange(@this.GetInterfaces());

            return typeList;
        }
    }
}