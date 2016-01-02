// <copyright file="InformationBlock.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.ParsingElements.FortranBlocks
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text.RegularExpressions;
    using FortranObjects;
    using Helper;
    using Parsing;
    using Utilitys;

    public class InformationBlock : IFortranBlock
    {
        private readonly int _depth;

        private readonly IDictionary<string, IInformationFactory> _factories;

        public InformationBlock(int depth)
        {
            _depth = depth;
            _factories = new Dictionary<string, IInformationFactory>();
        }

        public InformationBlock(int depth, IDictionary<string, IInformationFactory> factoryDictionary)
            : this(depth)
        {
            _factories = factoryDictionary;
        }

        public bool CheckInternal => true;

        public bool ExplicitEnd => false;

        public string Name => $"Information({_depth})";

        public static IEnumerable<InformationBlock> MultiDepthEnumeration(int fromDepth, int toDepth)
        {
            Debug.Assert(fromDepth > 0, $"'{nameof(fromDepth)}' must be greater than or equal to 0.");

            return
                from i in Enumerable.Range(fromDepth, toDepth)
                select new InformationBlock(i);
        }

        public bool BlockEnd(IEnumerable<IFortranBlock> ancestors, List<FileLine> lines, int lineIndex)
        {
            if (lineIndex + 1 >= lines.Count)
            {
                return true;
            }

            return
                CommentUtils.InfoEnd(lines[lineIndex + 1].Text)
                || (CommentUtils.InfoStart(lines[lineIndex + 1].Text) && CommentUtils.InfoDepth(lines[lineIndex + 1].Text) <= _depth)
                || CommentUtils.NDescStart(lines[lineIndex + 1].Text);
        }

        public bool BlockStart(IEnumerable<IFortranBlock> ancestors, List<FileLine> lines, int lineIndex)
        {
            return
                CommentUtils.InfoAtDepthStart(lines[lineIndex].Text, _depth)
                && !CommentUtils.NDescStart(lines[lineIndex].Text);
        }

        public IEnumerable<IContained> ReturnObject(IEnumerable<IContained> subObjects, List<FileLine> lines)
        {
            // Regex group the type-name and it's value.
            var aMatch = Regex.Match(lines[0].Text.Trim(), @"!>+?\s*?(\w+)\s*?:\s*?(.*)");

            // Retrieve the type name
            var typeName = aMatch.Groups[1].Value.Trim();

            var subObjectList = subObjects.Cast<IInformation>().ToList();

            // Retrieve the value, from the definition line and any subsequent lines.
            var value = aMatch.Groups[2].Value.Trim()
                        + string.Concat(lines.Skip(1)
                            .Where(line => line.Number <= (subObjectList.Any() ? subObjectList.First().Lines.First().Number - 1 : lines.Last().Number))
                            .Select(line => line.Text.Substring(_depth + 1) + Environment.NewLine));

            // Seperate cases.
            //  0 - NO subobjects, NO value.
            //  1 - HAS subobjects, NO value.
            //  2 - HAS value, NO subobjects.
            //  3 - HAS value, HAS subobjects.
            var caseNum = Convert.ToInt32(subObjectList.Any()) + (2 * Convert.ToInt32(!value.IsNullOrEmpty()));

            switch (caseNum)
            {
                case 0:
                    throw new BlockParserException("An information block must contain either a value or sub-information elements.");
                case 1:
                    return CollectionUtils.Singlet(new InformationGroup(_depth, typeName, subObjectList, lines));
                case 2:
                    return
                        _factories.ContainsKey(typeName)
                            ? _factories[typeName].Create(_depth, value, subObjectList, lines).Cast<LinedInternal>()
                            : CollectionUtils.Singlet<IContained>(new InformationValue(_depth, typeName, value, lines));
                case 3:
                    throw new BlockParserException("An information block cannot contain both a value and sub-information elements.");
                default:
                    throw new ApplicationException("Unknown error in informational element.");
            }
        }
    }
}