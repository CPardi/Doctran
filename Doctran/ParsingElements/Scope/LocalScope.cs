// <copyright file="LocalScope.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.ParsingElements.Scope
{
    using Functional.Maybe;
    using Parsing;

    public abstract class LocalScope : Scope
    {
        protected LocalScope(IFortranObject obj)
            : base(obj)
        {
        }

        protected IScope ParentScope => (this.Object as IContained)?.AncestorOfType<IHasScope>().Select(ihs => ihs.Scope).OrElseDefault();

        public override bool Exists<T>(IIdentifier identifier)
            => this.ExistsInLocalStorage<T>(identifier) || (this.ParentScope?.Exists<T>(identifier) ?? false);

        public override bool GetObjectByIdentifier<T>(IIdentifier identifier, out T obj)
            => this.GetObjectFromLocalStorage(identifier, out obj) || (this.ParentScope?.GetObjectByIdentifier(identifier, out obj) ?? false);
    }
}