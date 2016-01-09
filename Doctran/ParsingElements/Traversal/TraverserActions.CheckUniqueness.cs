// <copyright file="TraverserActions.CheckUniqueness.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.ParsingElements.Traversal
{
    using System.Linq;
    using Parsing;
    using Utilitys;

    public static partial class TraverserActions
    {
        /// <summary>
        ///     Returns a <see cref="ITraverserAction" /> action that ensures that only zero or one occurence instances of
        ///     <typeparamref name="T" /> appear in <see cref="IContainer.SubObjects" />.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <returns>An <see cref="ITraverserAction{T}" />.</returns>
        public static ITraverserAction CheckUniqueness<T>()
            where T : IContained
        {
            return new TraverserAction<T>(
                obj =>
                {
                    // Get all the parent SubObjects, and return if
                    //  - Zero or one subobjects are found.
                    //  - This is not the first item in the list.
                    var ts = obj.Parent.SubObjects.Where(o => o is T).ToList();
                    if (ts.Count <= 1 || ts.First() != obj as IContained)
                    {
                        return;
                    }

                    // If we are here then the type is not unique.
                    // Remove this object from the list and remove the rest from the parent's SubObjects.
                    ts.Remove(obj);
                    obj.Parent.RemoveSubObjects(ts);

                    // Create text specifying error lines in the exception message. For example 10 to 20 and 30 to 31.
                    var textForIgnored = ts
                        .OrderBy(o => (o as IHasLines ?? o.Parent as IHasLines)?.Lines.First().Number)
                        .Select(o =>
                        {
                            var first = (o as IHasLines ?? o.Parent as IHasLines)?.Lines.First().Number;
                            var last = (o as IHasLines ?? o.Parent as IHasLines)?.Lines.Last().Number;
                            return first != last ? $"{first} to {last}" : $"{first}";
                        })
                        .DelimiteredConcat(", ", " and ");

                    // Create the exception message and throw an exception.
                    var message = $"{obj.ObjectName.ToUpperFirstLowerRest()} must be unique."
                                  +
                                  (textForIgnored.IsNullOrEmpty()
                                      ? string.Empty
                                      : $" Duplicate {obj.ObjectName.ToLower()}(s) appear at lines {textForIgnored} and have been ignored.");
                    throw new TraverserException(obj, message);
                });
        }
    }
}