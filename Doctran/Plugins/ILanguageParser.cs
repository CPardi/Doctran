// <copyright file="ILanguageParser.cs" company="Christopher Pardi">
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
    using Helper;
    using Parsing;

    public interface ILanguageParser
    {
        IPreprocessor Preprocessor { get; }

        IEnumerable<FortranBlock> BlocksParsers { get; }

        IEnumerable<Traverser> Traversers { get; }
    }

    public interface IPreprocessor
    {
        List<FileLine> Preprocess(List<FileLine> lines, string currentDirectory);
    }

    public class Preprocessor : IPreprocessor
    {
        private readonly IEnumerable<Func<List<FileLine>, string, List<FileLine>>> _preprocesses;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="preprocesses">The preprocessors to be applied. The order given will be the order of application.</param>
        public Preprocessor(params Func<List<FileLine>, string, List<FileLine>>[] preprocesses)
        {
            _preprocesses = preprocesses;
        }

        public List<FileLine> Preprocess(List<FileLine> lines, string currentDirectory)
            => _preprocesses.Aggregate(lines, (current, preprocess) => preprocess(current, currentDirectory));
    }
}