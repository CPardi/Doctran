// <copyright file="Identifier.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.ParsingElements
{
    /// <summary>
    ///     Default implementation of <see cref="IIdentifier" />. It defines a case-sensitive identifier.
    /// </summary>
    public class Identifier : IIdentifier
    {
        public Identifier(string identifier)
        {
            this.OriginalString = identifier;
        }

        public string OriginalString { get; }

        public Identifier CreateAlias(string newIdentifier) => new Identifier(newIdentifier);

        public override bool Equals(object obj) => this.OriginalString.Equals((obj as IIdentifier)?.OriginalString);

        public override int GetHashCode() => this.OriginalString.GetHashCode();

        public override string ToString() => this.OriginalString;

        IIdentifier IIdentifier.CreateAlias(string newIdentifier) => this.CreateAlias(newIdentifier);
    }
}