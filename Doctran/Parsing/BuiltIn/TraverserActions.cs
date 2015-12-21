// <copyright file="TraverserActions.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Parsing.BuiltIn
{
    using System.Linq;
    using FortranObjects;
    using Reporting;

    public static class TraverserActions
    {
        /// <summary>
        ///     Moves named descriptions in to the container or containers with identifier specified by linkedTo.
        /// </summary>
        public static ITraverserAction AssignDescriptions
        {
            get
            {
                return new TraverserAction<NamedDescription>(
                    obj =>
                    {
                        var linkedTo = obj?.LinkedTo;

                        if (linkedTo == null)
                        {
                            return;
                        }

                        // If this is a description directly below the definition statement then dont move it. This is really
                        // just for function where the result name is the same as the function name.
                        if ((obj.Parent as IHasIdentifier)?.Identifier == linkedTo
                            && obj.Parent.Lines.Count > 1 && obj.Parent.Lines[1].Number == obj.Lines[0].Number)
                        {
                            return;
                        }

                        var objsForDescription =
                            obj.Parent.SubObjects
                                .Where(sObjs => (sObjs as IHasIdentifier)?.Identifier == linkedTo);

                        obj.Parent.SubObjects.Remove(obj);
                        foreach (var match in objsForDescription)
                        {
                            match.AddSubObject(obj);
                        }
                    });
            }
        }

        public static ITraverserAction CheckDescriptionValidity
        {
            get
            {
                return new TraverserAction<NamedDescription>(
                    obj =>
                    {
                        CorrectName(obj);
                        CheckUniqueness(obj);
                    });
            }
        }

        private static void CheckUniqueness(IFortranObject obj)
        {
            if (obj.Parent.SubObjects.OfType<IDescription>().Count() <= 1)
            {
                return;
            }

            var curObj = obj;
            var file = obj.GoUpTillType<ISource>() as SourceFile;
            if (obj.Parent is Project)
            {
                Report.Warning(pub =>
                {
                    pub.AddWarningDescription("Description meta-data was ignored");
                    pub.AddReason("Multiple descriptions specified for a single block.");
                    pub.AddLocation(curObj.Lines.First().Number == curObj.Lines.Last().Number
                        ? $"At line {curObj.Lines.First().Number} of '{file?.Name}{file?.Extension}'."
                        : $"Within lines {curObj.Lines.First().Number} to {curObj.Lines.Last().Number} of '{file?.Name}{file?.Extension}'.");
                });
            }
            else
            {
                Report.Warning(pub =>
                {
                    pub.AddWarningDescription("Description meta-data was ignored");
                    pub.AddReason("Multiple descriptions specified for a single block.");
                    pub.AddLocation(curObj.Lines.First().Number == curObj.Lines.Last().Number
                        ? $"At line {curObj.Lines.First().Number} of '{file?.Name}{file?.Extension}'."
                        : $"Within lines {curObj.Lines.First().Number} to {curObj.Lines.Last().Number} of '{file?.Name}{file?.Extension}'.");
                });
            }
            obj.Parent.SubObjects.Remove(obj);
        }

        private static void CorrectName(FortranObject obj)
        {
            var desc = (NamedDescription)obj;

            if ((obj.Parent as IHasIdentifier)?.Identifier == desc.LinkedTo)
            {
                return;
            }

            var curObj = obj;
            var file = obj.GoUpTillType<ISource>();
            Report.Warning(pub =>
            {
                pub.AddWarningDescription("Description meta-data was ignored");
                pub.AddReason("Description identifier does not match parent identifier.");
                pub.AddLocation(curObj.Lines.First().Number == curObj.Lines.Last().Number
                    ? $"At line {curObj.Lines.First().Number} of '{(file as SourceFile)?.Name}{(file as SourceFile)?.Extension}'."
                    : $"Within lines {curObj.Lines.First().Number} to {curObj.Lines.Last().Number} of '{(file as SourceFile)?.Name}{(file as SourceFile)?.Extension}'.");
            });
            obj.Parent.SubObjects.Remove(obj);
        }
    }
}