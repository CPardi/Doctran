// <copyright file="LessFileCopier.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Output.Assets
{
    using System.Collections.ObjectModel;
    using dotless.Core;
    using Utilitys;

    internal class LessFileCopier : TextFileCopier
    {
        public override ReadOnlyCollection<string> FromExtensions => new[] { ".less", ".LESS" }.ToReadOnlyCollection();

        public override string ToExtension => ".css";

        protected override string ReadFile(string filePath)
        {
            return Less.Parse(base.ReadFile(filePath));
        }
    }
}