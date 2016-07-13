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
    using System.Linq;
    using Utilitys;

    /// <summary>
    ///     Manages documentation defintions added by plugins for the entire Doctran project.
    /// </summary>
    public static class DocumentationManager
    {
        /// <summary>
        ///     Internal storage of documentation definitions, keyed by file extension.
        /// </summary>
        private static readonly Dictionary<string, IDocumentationDefinition> DefinitionsByExtension = new Dictionary<string, IDocumentationDefinition>();

        /// <summary>
        ///     Internal storage of documentation definitions, keyed by identifier.
        /// </summary>
        private static readonly Dictionary<string, IDocumentationDefinition> DefinitionsByIdentifier = new Dictionary<string, IDocumentationDefinition>();

        /// <summary>
        ///     Gets the name of the part of the theme required for all projects.
        /// </summary>
        public static string BaseThemePart => "base";

        /// <summary>
        ///     Register a new documentation defineition to an identifier and file extension.
        /// </summary>
        /// <param name="identifier">The identifier to assign to the documentation definition.</param>
        /// <param name="extension">The extension to assign to the documentation definition.</param>
        /// <param name="documentationDefinition">The documentation definition.</param>
        public static void RegisterDocumentationDefinition(string identifier, string extension, IDocumentationDefinition documentationDefinition)
        {
            DefinitionsByIdentifier.Remove(identifier);
            DefinitionsByIdentifier.Add(identifier, documentationDefinition);

            DefinitionsByExtension.Remove(extension);
            DefinitionsByExtension.Add(PathUtils.DottedExtension(extension), documentationDefinition);
        }

        /// <summary>
        ///     Get a list of names of the required parts of the theme, given the documentation definitions registered and the
        ///     source file extensions specified. This includes <see cref="BaseThemePart" />.
        /// </summary>
        /// <param name="extensions">The source file extensions. Only distinct values will be considered.</param>
        /// <returns>The name of the parts of the theme required.</returns>
        /// <remarks>These should be used to define the directories copied to the output directory.</remarks>
        public static IEnumerable<string> RequiredThemeParts(IEnumerable<string> extensions)
        {
            var otherThemeParts = extensions
                ?.Distinct()
                .SelectMany(ext => TryGetDefinitionByExtension(ext).ThemePartNames)
                .Where(tpn => tpn != null)
                .Distinct()
                .ToArray();

            return new[] { BaseThemePart }.Concat(otherThemeParts);
        }

        /// <summary>
        ///     Removes all documentation definitions from the manager.
        /// </summary>
        public static void Reset()
        {
            DefinitionsByIdentifier.Clear();
        }

        public static IDocumentationDefinition TryGetDefinitionByExtension(string extension)
        {
            IDocumentationDefinition documentationDefinition;
            if (DefinitionsByExtension.TryGetValue(PathUtils.DottedExtension(extension), out documentationDefinition))
            {
                return documentationDefinition;
            }

            throw new ApplicationException($"A documentation generator is not defined for the file extension '{extension}'.");
        }

        public static IDocumentationDefinition TryGetDefinitionByIdentifier(string identifier)
        {
            IDocumentationDefinition documentationDefinition;
            if (DefinitionsByIdentifier.TryGetValue(identifier, out documentationDefinition))
            {
                return documentationDefinition;
            }

            throw new ApplicationException($"A documentation generator is not defined for the language identifier '{identifier}'.");
        }
    }
}