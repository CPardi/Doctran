// <copyright file="PathList.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Helper
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class PathList : IList<string>
    {
        private IList<string> _filePaths = new List<string>();

        public PathList()
            : this(true)
        {
        }

        public PathList(bool absolutePaths)
        {
            this.AbsolutePaths = absolutePaths;
        }

        private enum PathType
        {
            Normal,

            WildCard,

            RecursiveWildCard
        }

        public bool AbsolutePaths { get; }

        public int Count
        {
            get { return _filePaths.Count; }
        }

        public bool IsReadOnly
        {
            get { return _filePaths.IsReadOnly; }
        }

        string IList<string>.this[int index]
        {
            get { return _filePaths[index]; }
            set { _filePaths[index] = value; }
        }

        /// <summary>
        ///     Add a path that contains wildcards to the collection. Any wildcards are expaneded and resulting file path list is
        ///     added to the collection.
        /// </summary>
        /// <param name="path">A path that can contain wildcards.</param>
        public void Add(string path)
        {
            foreach (var expandedPath in ExpandedPath(path))
            {
                _filePaths.Add(expandedPath);
            }
        }

        public void Clear()
        {
            _filePaths.Clear();
        }

        public bool Contains(string path)
        {
            if (GetPathType(path) != PathType.Normal)
            {
                throw new ArgumentException("THe path must be of expanded type and not contain wilcard statements.");
            }
            return _filePaths.Contains(path);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            _filePaths.CopyTo(array, arrayIndex);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _filePaths.GetEnumerator();
        }

        public int IndexOf(string path)
        {
            if (GetPathType(path) != PathType.Normal)
            {
                throw new ArgumentException("THe path must be of expanded type and not contain wilcard statements.");
            }
            return _filePaths.IndexOf(path);
        }

        public void Insert(int index, string path)
        {
            foreach (var expandedPath in ExpandedPath(path))
            {
                _filePaths.Insert(index, expandedPath);
            }
        }

        public void KeepDistinctOnly()
        {
            _filePaths = _filePaths.Distinct().ToList();
        }

        public bool Remove(string path)
        {
            if (GetPathType(path) != PathType.Normal)
            {
                throw new ArgumentException("THe path must be of expanded type and not contain wilcard statements.");
            }
            return _filePaths.Remove(path);
        }

        public void RemoveAt(int index)
        {
            _filePaths.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _filePaths.GetEnumerator();
        }

        private string[] ExpandedPath(string path)
        {
            string[] filePathsFromExpansion;

            // Try to match wildcards, * and **
            var dirMatch = Regex.Match(path.Replace(@"\", "/"), @"^(.*?)\\?(\*\*)?\/?(\w*[\*\?]\w*(?:.\w+)?)?$");

            // Get the directory to search within.
            var normalPath = dirMatch.Groups[1].Value == "" ? "." : dirMatch.Groups[1].Value;
            if (this.AbsolutePaths)
            {
                normalPath = Path.GetFullPath(normalPath);
            }

            // We have a recusive wildcard match.
            if (dirMatch.Groups[2].Value != "")
            {
                filePathsFromExpansion = Directory.GetFiles(normalPath, dirMatch.Groups[3].Value, SearchOption.AllDirectories)
                    .Select(p => p.StartsWith(@"./") | p.StartsWith(@".\") ? p.Substring(2).Replace(@"\", "/") : p.Replace(@"\", "/")).ToArray();
            }

            // We have a non-recusive (top level only) wildcard match.
            else if (dirMatch.Groups[3].Value != "")
            {
                filePathsFromExpansion = Directory.GetFiles(normalPath, dirMatch.Groups[3].Value, SearchOption.TopDirectoryOnly)
                    .Select(p => p.StartsWith(@"./") | p.StartsWith(@".\") ? p.Substring(2).Replace(@"\", "/") : p.Replace(@"\", "/")).ToArray();
            }

            // We have a bog standard file path, with no wildcards.
            else
            {
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException($"Could not find file '{path}'.");
                }
                filePathsFromExpansion = new[] { normalPath };
            }

            return filePathsFromExpansion;
        }

        private PathType GetPathType(string path)
        {
            // Try to match wildcards, * and **
            var dirMatch = Regex.Match(path.Replace(@"\", "/"), @"^(.*?)\\?(\*\*)?\/?(\w*[\*\?]\w*(?:.\w+)?)?$");
            if (dirMatch.Groups[2].Value != "")
            {
                return PathType.RecursiveWildCard;
            }
            if (dirMatch.Groups[3].Value != "")
            {
                return PathType.WildCard;
            }
            return PathType.Normal;
        }
    }
}