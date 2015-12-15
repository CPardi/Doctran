// <copyright file="InformationBlock.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing.BuiltIn.FortranBlocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Comments;
    using FortranObjects;
    using Helper;
    using Utilitys;

    public class InformationBlock : FortranBlock
    {
        private readonly int _depth;

        private readonly IDictionary<string, IInformationFactory> _factories;

        public InformationBlock(int depth)
            : base("Information_" + depth, true, false, 1)
        {
            _depth = depth;
            _factories = new Dictionary<string, IInformationFactory>();
        }

        public InformationBlock(int depth, IDictionary<string, IInformationFactory> factoryDictionary)
            : this(depth)
        {
            _factories = factoryDictionary;
        }

        public static IEnumerable<InformationBlock> MultiDepthEnumeration(int fromDepth, int toDepth)
        {
            return
                from i in Enumerable.Range(fromDepth, toDepth)
                select new InformationBlock(i);
        }

        public void AddFactory(string matchType, IInformationFactory factory)
        {
            if (_factories.ContainsKey(matchType))
            {
                throw new Exception(); // Change this to another exception type.
            }
            _factories.Add(matchType, factory);
        }

        public override bool BlockEnd(string parentBlockName, List<FileLine> lines, int lineIndex)
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

        public override bool BlockStart(string parentBlockName, List<FileLine> lines, int lineIndex)
        {
            return
                CommentUtils.InfoAtDepthStart(lines[lineIndex].Text, _depth)
                && !CommentUtils.NDescStart(lines[lineIndex].Text);
        }

        public override IEnumerable<FortranObject> ReturnObject(IEnumerable<IFortranObject> subObjects, List<FileLine> lines)
        {
            // Regex group the type-name and it's value.
            var aMatch = Regex.Match(lines[0].Text.Trim(), @"!>+?\s*?(\w+)\s*?:\s*?(.*)");

            // Retrieve the type name
            var typeName = aMatch.Groups[1].Value.Trim();

            // Retrieve the value, from the definition line and any subsequent lines.
            var value = aMatch.Groups[2].Value.Trim()
                        + string.Concat(lines.Skip(1)
                            .Where(line => line.Number <= (subObjects.Any() ? subObjects.First().Lines.First().Number - 1 : lines.Last().Number))
                            .Select(line => line.Text.Substring(_depth + 1) + Environment.NewLine));

            var objs = _factories.ContainsKey(typeName)
                ? _factories[typeName].Create(_depth, value, subObjects, lines).Cast<FortranObject>()
                : CollectionUtils.Singlet(new XInformation(_depth, typeName, value, subObjects, lines));
            return objs.ToList();
        }
    }

    //public class SubInformation : XFortranObject
    //{
    //    public string Value { get; private set; }

    //    public SubInformation(string typename, string value)
    //        : base(typename, new List<FileLine>())
    //    {
    //        Value = value;
    //    }

    //    public SubInformation(string typename, string value, IEnumerable<FortranObject> sub_objects, List<FileLine> lines)
    //        : base(typename, sub_objects, lines)
    //    {
    //        Value = value;
    //    }

    //    protected override string GetIdentifier()
    //    {
    //        return "SubInformation(" + this.XElement_Name + ")";
    //    }

    //    public override XElement XEle()
    //    {
    //        return Value != "" ?
    //            XElement.Parse("<" + this.XElement_Name + ">" + Value + @"</" + this.XElement_Name + ">")
    //            : new XElement(this.XElement_Name, this.SubObjects.Select(sinfo => (sinfo as SubInformation).XEle()));
    //    }
    //}
}