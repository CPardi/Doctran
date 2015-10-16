//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

using Doctran.BaseClasses;

namespace Doctran.Fbase.Common
{
    public class AssemblyLoader
    {
        private readonly List<Type> _assemblyTypes = new List<Type>();

        public AssemblyLoader(string pluginPath)
        {
            _assemblyTypes.AddRange(typeof(FortranObject).Assembly.GetTypes());
            string[] paths = null;
            try
            {
                paths = Directory.GetFiles(Path.GetFullPath(pluginPath));
            }
            catch (IOException e)
            {
                UserInformer.GiveWarning("plugin directory", e);
                return;
            }

            foreach (string path in paths)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(path);
                    this._assemblyTypes.AddRange(assembly.GetTypes());
                }
                catch (IOException e)
                {
                    UserInformer.GiveWarning("loaded plugin", e);
                    return;
                }
            }
            this._assemblyTypes.OrderByDescending(t => t.Name);
        }

        public List<T> GetClassInstances<T>()
        {
            return GetClassInstances<T>(new Func<T,int>(weight => 0), name => true);
        }

        public List<T> GetClassInstances<T>(Func<T, int> ordering)
        {
            return GetClassInstances<T>(ordering, name => true);
        }

        public List<T> GetClassInstances<T>(string fromNamespace)
        {
            return GetClassInstances<T>(new Func<T, int>(weight => 0), name => name.StartsWith(fromNamespace));
        }

        private List<T> GetClassInstances<T>(Func<T, int> ordering, Func<string, bool> namespaceWhere)
        {
            List<T> instances = new List<T>();
            var typesOfT = this._assemblyTypes.Where(t => !t.IsAbstract && (t.IsSubclassOf(typeof(T)) | t.GetInterfaces().Contains(typeof(T)) ));
            instances.AddRange(
                from t in typesOfT
                where namespaceWhere(t.Namespace)
                where !t.IsInterface
                let instOfT = (T)Activator.CreateInstance(t)
                orderby ordering(instOfT)
                select instOfT
            );
            return instances;
        }

        public List<Type> GetClassTypes<T>()
        {
            return GetClassTypes<T>(new Func<Type, int>(weight => 0), name => true);
        }

        public List<Type> GetClassTypes<T>(Func<Type, int> ordering)
        {
            return GetClassTypes<T>(ordering, name => true);
        }

        public List<Type> GetClassTypes<T>(string fromNamespace = "")
        {
            return GetClassTypes<T>(new Func<Type, int>(weight => 0), name => name.StartsWith(fromNamespace));
        }

        private List<Type> GetClassTypes<T>(Func<Type, int> ordering, Func<string, bool> namespaceWhere)
        {
            List<Type> types = new List<Type>();
            var typesOfT = this._assemblyTypes.Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(T)));
            types.AddRange(
                from t in typesOfT
                where namespaceWhere(t.Namespace)
                orderby ordering(t)
                select t
            );
            return types;
        }
    }
}

