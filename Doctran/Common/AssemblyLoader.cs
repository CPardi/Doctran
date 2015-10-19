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
    using Reporting;

    public class AssemblyLoader
    {
        private readonly List<Type> _assemblyTypes = new List<Type>();

        public AssemblyLoader(string pluginPath)
        {
            _assemblyTypes.AddRange(typeof(FortranObject).Assembly.GetTypes());
            string[] paths;
            try
            {
                paths = Directory.GetFiles(Path.GetFullPath(pluginPath));
            }
            catch (IOException e)
            {
                Report.Warning(pub =>
                {
                    pub.AddWarningDescription("Plugin directory is invalid.");
                    pub.AddReason(e.Message);
                    pub.AddLocation(pluginPath);
                });

                return;
            }

            foreach (var path in paths)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(path);
                    _assemblyTypes.AddRange(assembly.GetTypes());
                }
                catch (IOException e)
                {
                    Report.Warning(pub =>
                    {
                        pub.AddWarningDescription("Could not load plugin.");
                        pub.AddReason(e.Message);
                        pub.AddLocation(pluginPath);
                    });
                    return;
                }
            }

            _assemblyTypes = _assemblyTypes.OrderByDescending(t => t.Name).ToList();
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

