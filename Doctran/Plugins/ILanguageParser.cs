﻿// <copyright file="ILanguageParser.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Plugins
{
    using System.Collections.ObjectModel;
    using Parsing;
    using ParsingElements;

    public interface ILanguageParser
    {
        string FriendlyName { get; }

        ReadOnlyCollection<ITraverserAction> GlobalTraverserActions { get; }

        string Identifier { get; }

        ISourceFile Parse(string sourcePath, string lines);
    }
}