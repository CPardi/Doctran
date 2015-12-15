// <copyright file="OptionFactory.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Input.OptionFile
{
    using System.Collections.Generic;
    using Comments;
    using Helper;
    using Parsing;

    internal class OptionFactory : IInformationFactory
    {
        public OptionFactory(string name)
        {
            this.Name = name;
        }

        public string Name { get; }

        public IEnumerable<IInformation> Create(int depth, string value, IEnumerable<IFortranObject> subObjects, List<FileLine> lines)
        {
            yield return new Option(depth, this.Name, value, lines);
        }
    }

    internal class Option : FortranObject, IInformation
    {
        public Option(int depth, string name, string value, List<FileLine> lines)
            : base(lines)
        {
            this.Name = name;
            this.Value = value;
            this.Depth = depth;
        }

        public int Depth { get; }

        public string Name { get; }

        public string Value { get; }
    }
}