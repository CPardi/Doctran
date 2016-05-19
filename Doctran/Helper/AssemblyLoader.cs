// <copyright file="AssemblyLoader.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Helper
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Parsing;
    using ParsingElements;
    using Reporting;

    public class AssemblyLoader
    {
        private readonly List<Type> _assemblyTypes = new List<Type>();

        public AssemblyLoader(string pluginPath)
        {
            _assemblyTypes.AddRange(typeof(LinedInternal).Assembly.GetTypes());
            string[] paths;
            try
            {
                paths = Directory.GetFiles(Path.GetFullPath(pluginPath));
            }
            catch (IOException e)
            {
                Report.Warning(pub => pub.DescriptionReasonLocation(ReportGenre.Plugin, $"Plugin directory is invalid. {e.Message}", pluginPath), e);
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
                    Report.Warning(pub => pub.DescriptionReasonLocation(ReportGenre.Plugin, $"Could not load plugin. {e.Message}", pluginPath), e);
                    return;
                }
            }

            _assemblyTypes = _assemblyTypes.OrderByDescending(t => t.Name).ToList();
        }

        public List<T> GetClassInstances<T>()
        {
            return this.GetClassInstances<T>(weight => 0, name => true);
        }

        public List<T> GetClassInstances<T>(Func<T, int> ordering)
        {
            return this.GetClassInstances(ordering, name => true);
        }

        public List<T> GetClassInstances<T>(string fromNamespace)
        {
            return this.GetClassInstances<T>(weight => 0, name => name.StartsWith(fromNamespace));
        }

        public List<Type> GetClassTypes<T>()
        {
            return this.GetClassTypes<T>(weight => 0, name => true);
        }

        public List<Type> GetClassTypes<T>(Func<Type, int> ordering)
        {
            return this.GetClassTypes<T>(ordering, name => true);
        }

        public List<Type> GetClassTypes<T>(string fromNamespace)
        {
            return this.GetClassTypes<T>(weight => 0, name => name.StartsWith(fromNamespace));
        }

        private List<T> GetClassInstances<T>(Func<T, int> ordering, Func<string, bool> namespaceWhere)
        {
            var instances = new List<T>();
            var typesOfT = _assemblyTypes.Where(t => !t.IsAbstract && (t.IsSubclassOf(typeof(T)) | t.GetInterfaces().Contains(typeof(T))));
            instances.AddRange(
                from t in typesOfT
                where namespaceWhere(t.Namespace)
                where !t.IsInterface
                let instOfT = (T)Activator.CreateInstance(t)
                orderby ordering(instOfT)
                select instOfT);
            return instances;
        }

        private List<Type> GetClassTypes<T>(Func<Type, int> ordering, Func<string, bool> namespaceWhere)
        {
            var types = new List<Type>();
            var typesOfT = _assemblyTypes.Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(T)));
            types.AddRange(
                from t in typesOfT
                where namespaceWhere(t.Namespace)
                orderby ordering(t)
                select t);
            return types;
        }
    }
}