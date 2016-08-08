// <copyright file="IQuasiContainer.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing
{
    using System.Collections.Generic;

    /// <summary>
    ///     Exposes members that do not appear in <see cref="IContainer.SubObjects" />, but are generated dynamically. Used to
    ///     allow generic searching of project structure , such as in
    ///     <see cref="Traversal.Find{T}(IContainer, ParsingElements.IIdentifier)" />.
    /// </summary>
    public interface IQuasiContainer : IFortranObject
    {
        /// <summary>
        ///     Gets objects that do not appear in <see cref="IContainer.SubObjects" />, to allow access in a generic way.
        /// </summary>
        IEnumerable<IFortranObject> QuasiObjects { get; }
    }
}