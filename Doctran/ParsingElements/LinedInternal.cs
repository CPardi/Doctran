// <copyright file="LinedInternal.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.ParsingElements
{
    using System.Collections.Generic;
    using System.Linq;
    using FortranObjects;
    using Functional.Maybe;
    using Helper;
    using Parsing;

    public abstract class LinedInternal : Container, IContained, IHasDescription, IHasLines
    {
        protected LinedInternal(IEnumerable<IContained> subObjects, List<FileLine> lines)
            : base(subObjects)
        {
            this.Lines = lines;
        }

        public List<FileLine> Lines { get; }

        public IContainer Parent { get; set; }

        public virtual Maybe<IDescription> Description
            => this.SubObjectsOfType<Description>().Cast<IDescription>().FirstMaybe()
                .Or(() =>
                {
                    var objectIdentifier = (this as IHasIdentifier)?.Identifier;
                    if (objectIdentifier == null)
                    {
                        return Maybe<IDescription>.Nothing;
                    }

                    var d = (IDescription)this.Parent?.SubObjectsOfType<NamedDescription>().LastOrDefault(nd => Equals(objectIdentifier, nd.LinkedTo))
                            ?? (IDescription)this.SubObjectsOfType<NamedDescription>().FirstOrDefault(nd => Equals(objectIdentifier, nd.LinkedTo));
                    return d.ToMaybe();
                });
    }
}