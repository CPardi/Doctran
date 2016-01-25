// <copyright file="TraverserActions.CheckTemporaries.cs" company="Christopher Pardi">
// Copyright (c) 2015 Christopher Pardi. All rights reserved.
// </copyright>

namespace Doctran.ParsingElements.Traversal
{
    using Parsing;

    public static partial class TraverserActions
    {
        /// <summary>
        ///     Returns a <see cref="ITraverserAction" /> action that ensures that instances of
        ///     <typeparamref name="T" /> do not appear. If found, then these instances are removed.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <returns>An <see cref="ITraverserAction{T}" />.</returns>
        public static ITraverserAction CheckForUnexpected<T>()
            where T : IContained
        {
            return new TraverserAction<T>(
                (obj, errLis) =>
                {
                    obj.Parent.RemoveSubObject(obj);
                    errLis.Error(new TraverserException(obj, $"An unexpected object of type '{obj.ObjectName}' was found in project and ignored."));
                });
        }
    }
}