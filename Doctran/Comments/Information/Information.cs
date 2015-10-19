//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

using Doctran.Fbase.Common;

using Doctran.BaseClasses;

namespace Doctran.Fbase.Comments
{
    public class InformationBlock : FortranBlock
    {
        private int _depth; 
        private IDictionary<string, IInformationFactory> _factories;

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

        public void AddFactory(string matchType, IInformationFactory factory)
        {
            if (_factories.ContainsKey(matchType)) throw new Exception(); // Change this to another exception type.
            _factories.Add(matchType, factory);
        }

        public override bool BlockStart(string parentBlockName, List<FileLine> lines, int lineIndex)
        {
            return
                CommentDefinitions.InfoAtDepthStart(lines[lineIndex].Text, _depth)
                && !CommentDefinitions.NDescStart(lines[lineIndex].Text);
        }

        public override bool BlockEnd(string parentBlockName, List<FileLine> lines, int lineIndex)
        {
            if (lineIndex + 1 >= lines.Count) return true;

            return
                CommentDefinitions.InfoEnd(lines[lineIndex + 1].Text)
                || (CommentDefinitions.InfoStart(lines[lineIndex + 1].Text) && CommentDefinitions.InfoDepth(lines[lineIndex + 1].Text) <= _depth)
                || CommentDefinitions.NDescStart(lines[lineIndex + 1].Text);
        }

        public override List<FortranObject> ReturnObject(IEnumerable<FortranObject> subObjects, List<FileLine> lines)
        {
            // Regex group the type-name and it's value.
            Match aMatch = Regex.Match(lines[0].Text.Trim(), @"!>+?\s*?(\w+)\s*?:\s*?(.*)");

            // Retrieve the type name
            string typeName = aMatch.Groups[1].Value.Trim();

            // Retrieve the value, from the definition line and any subsequent lines.
            string value = aMatch.Groups[2].Value.Trim()
                + string.Concat(lines.Skip(1)
                                .Where(line => line.Number <= (subObjects.Any() ? subObjects.First().lines.First().Number - 1 : lines.Last().Number))
                                .Select(line => line.Text.Substring(_depth + 1) + Environment.NewLine));

            IEnumerable<FortranObject> objs = _factories.ContainsKey(typeName)
                ?
                    _factories[typeName].Create(_depth, value, subObjects, lines).Cast<FortranObject>()
                :
                    HelperUtils.Singlet(new XInformation(_depth, typeName, value, subObjects, lines));
            return objs.ToList();            
        }
    }

    public interface IInformation {
        string Name { get; }
        int Depth { get; }
        List<FileLine> lines { get; }
        List<FortranObject> SubObjects { get; }
    }

    public interface IInformationFactory
    {
        IEnumerable<IInformation> Create(int depth, string value, IEnumerable<FortranObject> subObjects, List<FileLine> lines);
    }

    public class XInformation : XFortranObject, IInformation
    {
        public string Value { get; private set; }
        public int Depth { get; private set; }

        public XInformation(int depth, string name, string value, List<FileLine> lines)
            : base(name, lines)
        {
            this.Name = name;
            this.Value = value;
            this.Depth = depth;
        }

        public XInformation(int depth, string name, string value, IEnumerable<FortranObject> sub_objects, List<FileLine> lines)
            : base(name, sub_objects, lines)
        {
            this.Name = name;
            this.Value = value;
            this.Depth = depth;

        }

        protected override string GetIdentifier()
        {
            return "Information(" + this.XElement_Name + ")";
        }

        public override XElement XEle()
        {
            return (string.IsNullOrEmpty(Value)
                ?
                new XElement(this.XElement_Name, this.SubObjects.Select(sinfo => (sinfo as XInformation).XEle()))
                :
                XElement.Parse("<" + this.XElement_Name + ">" + this.Value + "</" + this.XElement_Name + ">")
                );
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