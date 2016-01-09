// <copyright file="PathList.cs" company="Christopher Pardi">
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
    using System.Text.RegularExpressions;
    using Utilitys;

    /// <summary>
    ///     Stores a list of file paths. Adding a new path can include wildcards, which are expanded and each resultant added
    ///     to the list.
    /// </summary>
    public class PathList : AbstractList<string>
    {
        public enum PathStorageMode
        {
            Absolute,

            Relative
        }

        /// <summary>
        ///     Gets or sets the mode of which paths are added to the path list.
        /// </summary>
        public PathStorageMode PathStorage { get; set; } = PathStorageMode.Absolute;

        /// <summary>
        ///     Adds a path to the list. Wildcards are expanded and resulting paths
        ///     are added.
        /// </summary>
        /// <param name="path">A path that can contain wildcards.</param>
        public override void Add(string path)
        {
            foreach (var p in this.ExpandPath(path))
            {
                this.InternalList.Add(p);
            }
        }

        public void AddRange(IEnumerable<string> paths)
        {
            foreach (var path in paths)
            {
                this.Add(path);
            }
        }

        /// <summary>
        ///     Determines whether <see cref="PathList" /> contains a single path.
        /// </summary>
        /// <param name="path">The path string to check its existance within the list.</param>
        /// <returns>If true, then the item appears within the list.</returns>
        /// <exception cref="ArgumentException">Thrown when the path contains wildcard statements.</exception>
        public override bool Contains(string path)
        {
            if (!this.IsSinglePath(path))
            {
                throw new ArgumentException("The path must be of expanded type and not contain wildcard statements.");
            }

            return this.InternalList.Contains(path);
        }

        /// <summary>
        ///     Removes a single path from the list.
        /// </summary>
        /// <param name="path">The single path to remove.</param>
        /// <returns>If true, then the path was found and remove from the list.</returns>
        /// <exception cref="ArgumentException">Thrown when the path contains wildcard statements.</exception>
        public override bool Remove(string path)
        {
            if (!this.IsSinglePath(path))
            {
                throw new ArgumentException("The path must be of expanded type and not contain wildcard statements.");
            }

            return this.InternalList.Remove(path);
        }

        /// <summary>
        ///     Analyses a path for wildcards.
        /// </summary>
        /// <param name="pathList">The path string to analyse.</param>
        /// <param name="path">The directory to search within.</param>
        /// <param name="searchPattern">
        ///     The search pattern implied by <paramref name="pathList" />. If equal to
        ///     <see cref="string.Empty" />, then the <paramref name="pathList" /> is a single path.
        /// </param>
        /// <param name="searchOption">The search method implied by <paramref name="pathList" />.</param>
        private void AnalysePath(string pathList, out string path, out string searchPattern, out SearchOption searchOption)
        {
            var dirtext = Regex.Match(
                pathList,
                @"^(.*?)\\?(\*\*)?\\?(\*(?:.\w+)?)?$".Replace(@"\\", @"\" + EnvVar.Slash));

            path = dirtext.Groups[1].Value == string.Empty ? "." : dirtext.Groups[1].Value;
            searchPattern = dirtext.Groups[3].Value;
            searchOption = dirtext.Groups[2].Value != string.Empty
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly;
        }

        /// <summary>
        ///     Expands <paramref name="pathList" /> to an <see cref="IEnumerable{String}" /> containing the expanded path.
        /// </summary>
        /// <param name="pathList">A path containing wildcards.</param>
        /// <returns>The expanded paths.</returns>
        private IEnumerable<string> ExpandPath(string pathList)
        {
            string path, searchPattern;
            SearchOption searchOption;

            this.AnalysePath(pathList, out path, out searchPattern, out searchOption);

            if (searchPattern == string.Empty && !File.Exists(path))
            {
                throw new FileNotFoundException($"The file '{Path.GetFullPath(path)}' does not exist.");
            }

            if (searchPattern != string.Empty && !Directory.Exists(path))
            {
                throw new DirectoryNotFoundException($"The directory '{Path.GetFullPath(path)}' does not exist.");
            }

            var paths =
                searchPattern == string.Empty
                    ? new[] { path }
                    : Directory.GetFiles(path, searchPattern, searchOption);

            switch (this.PathStorage)
            {
                case PathStorageMode.Absolute:
                    return paths.Select(Path.GetFullPath);
                case PathStorageMode.Relative:
                    return paths;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     Determines when <paramref name="pathList" /> is a single path (does not contain wildcards).
        /// </summary>
        /// <param name="pathList">The path to check.</param>
        /// <returns>If true, then <paramref name="pathList" />is a single path.</returns>
        private bool IsSinglePath(string pathList)
        {
            string path, searchPattern;
            SearchOption searchOption;

            this.AnalysePath(pathList, out path, out searchPattern, out searchOption);

            return searchPattern == string.Empty;
        }
    }
}