// <copyright file="IFileCopier.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Output.Assets
{
    using System.Collections.ObjectModel;

    internal interface IFileCopier
    {
        ReadOnlyCollection<string> FromExtensions { get; }

        string ToExtension { get; }

        void Run(string inputPath, string outputPath, bool overwrite);
    }
}