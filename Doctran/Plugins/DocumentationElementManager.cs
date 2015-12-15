// <copyright file="DocumentationElementManager.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Plugins
{
    using System.Collections.Generic;
    using Parsing;

    public static class DocumentationElementManager
    {
        private static readonly List<IGroupXElement> GroupXElements = new List<IGroupXElement>();

        private static readonly List<IInterfaceXElements> InterfaceXElements = new List<IInterfaceXElements>();

        private static readonly List<IObjectXElement> ObjectXElements = new List<IObjectXElement>();

        public static IEnumerable<IInterfaceXElements> AllInterfaceXElements => InterfaceXElements;

        public static IEnumerable<IGroupXElement> AllObjectGroupXElements => GroupXElements;

        public static IEnumerable<IObjectXElement> AllObjectXElements => ObjectXElements;

        public static void RegisterNewGroupXElements(IEnumerable<IGroupXElement> groupXElements)
        {
            GroupXElements.AddRange(groupXElements);
        }

        public static void RegisterNewInterfaceXElements(IEnumerable<IInterfaceXElements> interfaceXElements)
        {
            InterfaceXElements.AddRange(interfaceXElements);
        }

        public static void RegisterNewObjectXElements(IEnumerable<IObjectXElement> objectXElements)
        {
            ObjectXElements.AddRange(objectXElements);
        }
    }
}