// <copyright file="DocumentationManager.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Plugins
{
    using System;
    using System.Collections.Generic;

    public static class DocumentationManager
    {
        private static readonly Dictionary<string, IXmlSerializer> DefinitionsByIdentifier = new Dictionary<string, IXmlSerializer>();

        public static void RegisterDocumentationDefinition(string identifier, string extension, IXmlSerializer documentationDefinition)
        {
            DefinitionsByIdentifier.Remove(identifier);
            DefinitionsByIdentifier.Add(identifier, documentationDefinition);
        }

        public static IXmlSerializer TryGetDefinitionByIdentifier(string identifier)
        {
            IXmlSerializer documentationDefinitions;
            if (DefinitionsByIdentifier.TryGetValue(identifier, out documentationDefinitions))
            {
                return documentationDefinitions;
            }

            throw new ApplicationException($"A parser is not defined for the language identifier '{identifier}'.");
        }
    }
}