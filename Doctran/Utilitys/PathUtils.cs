// <copyright file="PathUtils.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Utilitys
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    ///     A collection of function relating to path operations.
    /// </summary>
    public static class PathUtils
    {
        /// <summary>
        ///     Changes a path's files to a different extension.
        /// </summary>
        /// <param name="path">The path whose file extension is to be changed.</param>
        /// <param name="newExtension">The new file extension. The dot is optional.</param>
        /// <returns>The path with file extension <paramref name="newExtension" />.</returns>
        public static string ChangeExtension(string path, string newExtension)
        {
            if (newExtension == null)
            {
                return path;
            }

            var actualExtension = Path.GetExtension(path) ?? string.Empty;
            return $"{path.Substring(0, path.Length - actualExtension.Length)}{DottedExtension(newExtension)}";
        }

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
        ///     Returns a path with the filename and a specified number of directories.
        /// </summary>
        /// <param name="path">
        ///     A path that ends in a filename. Relative path will not be consider as absolute paths.
        /// </param>
        /// <param name="numDirectories">
        ///     The number of containing directories to include in the returned path. If 0, then the
        ///     filename is returned. If <paramref name="numDirectories" /> is larger that the number of containing directories,
        ///     then the original path is returned.
        /// </param>
        /// <returns>A path containing a filename and <paramref name="numDirectories" /> of its containing directories.</returns>
        public static string FilenameAndAncestorDirectories(string path, int numDirectories)
        {
            var seperator =
                path.Contains(Path.AltDirectorySeparatorChar)
                    ? Path.AltDirectorySeparatorChar
                    : Path.DirectorySeparatorChar;

            var directories = SplitIntoDirectories(path);

            var dirCount = directories.Length;

            return directories.Skip(dirCount - numDirectories - 1).Reverse().DelimiteredConcat(seperator.ToString());
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

        /// <summary>
        ///     Return an array containing each directory in a path. If the last element of a path is a file name, then the last
        ///     element of the return array is the filename.
        /// </summary>
        /// <param name="path">The path to be split.</param>
        /// <returns>An array of directories and if specified in the path a filename.</returns>
        public static string[] SplitIntoDirectories(string path)
        {
            return path.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        ///     Given a path, the number of ancestor directories is returned required to make each string unique.
        /// </summary>
        /// <param name="paths">An enumeration to path.</param>
        /// <returns>
        ///     The number of ancestor directories required to make each string within <paramref name="paths" /> unique. A
        ///     zero is return for cases where each filename is unique.
        /// </returns>
        /// <exception cref="InvalidOperationException">Thrown when two or more paths are identical.</exception>
        public static int UniquePathLevel(IEnumerable<string> paths)
        {
            var pathList = paths as IList<string> ?? paths.ToList();

            var max = pathList.Select(SplitIntoDirectories).Select(dirs => dirs.Length).Max();

            var n = -1;
            bool hasDuplicates;
            do
            {
                n++;
                hasDuplicates = pathList.GroupBy(p => FilenameAndAncestorDirectories(p, n)).Any(p => p.Count() > 1);

                if (n > max)
                {
                    throw new InvalidOperationException("Two or more paths are identical.");
                }
            }
            while (hasDuplicates);

            return n;
        }
    }
}