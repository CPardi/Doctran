// <copyright file="PathUtilitys.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Utilitys
{
    using System;
    using System.IO;

    /// <summary>
    ///     A collection of function relating to path operations.
    /// </summary>
    public class PathUtils
    {
        /// <summary>
        ///     If required, adds a leading period to a file extension.
        /// </summary>
        /// <param name="extension">A file extension.</param>
        /// <returns>The extension with a leading period.</returns>
        public static string DottedExtension(string extension)
        {
            return extension.StartsWith(".") ? extension : '.' + extension;
        }

        /// <summary>
        ///     Get the path relative to a directory.
        /// </summary>
        /// <param name="directory">The directory the path should be made relative to.</param>
        /// <param name="path">The path to be transformed.</param>
        /// <returns><paramref name="path" /> relative to <paramref name="directory" />.</returns>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="path" /> is not within the directory given by
        ///     <paramref name="directory" />.
        /// </exception>
        /// <remarks>Uses a simple substring method. Relies on no information from the file system.</remarks>
        public static string PathRelativeTo(string directory, string path)
        {
            var length = directory.EndsWith($"{EnvVar.Slash}") ? directory.Length : directory.Length + 1;
            if (path.Substring(0, directory.Length) != directory)
            {
                throw new ArgumentException($"The '{path}' is not within the directory '{directory}'.", nameof(directory));
            }

            return path.Substring(length);
        }

        /// <summary>
        ///     Run an action in a different directory.
        /// </summary>
        /// <param name="path">The path of the directory to run an action within.</param>
        /// <param name="action">The action to perform.</param>
        public static void RunInDirectory(string path, Action action)
        {
            var currentDirectory = Environment.CurrentDirectory;
            var dir = string.IsNullOrEmpty(path)
                ? currentDirectory
                : path;

            Environment.CurrentDirectory = dir;
            action();
            Environment.CurrentDirectory = currentDirectory;
        }

        public static string ChangeExtension(string path, string newExtension)
        {
            if (newExtension == null)
            {
                return path;
            }

            var actualExtension = Path.GetExtension(path) ?? string.Empty;
            return $"{path.Substring(0, path.Length - actualExtension.Length)}{DottedExtension(newExtension)}";
        }
    }
}