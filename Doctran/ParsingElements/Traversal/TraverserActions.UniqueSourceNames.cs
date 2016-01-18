// <copyright file="TraverserActions.UniqueSourceNames.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.ParsingElements.Traversal
{
    using System.Linq;
    using FortranObjects;
    using Parsing;
    using Utilitys;

    public static partial class TraverserActions
    {
        public static ITraverserAction UniqueSourceNames
        {
            get
            {
                return new TraverserAction<Project>(
                    obj =>
                    {
                        var uniquenessLevel = PathUtils.UniquePathLevel(obj.Sources.Select(s => s.AbsolutePath));
                        foreach (var s in obj.Sources)
                        {
                            s.NameUniquenessLevel = uniquenessLevel;
                        }
                    });
            }
        }
    }
}