namespace Doctran.Parsing.BuiltIn
{
    using System.Collections.Generic;
    using System.Linq;
    using FortranObjects;
    using Reporting;

    public static class TraverserActions
    {
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
                        if (obj.parent.Identifier == linkedTo
                            && obj.parent.lines.Count > 1 && obj.parent.lines[1].Number == obj.lines[0].Number)
                            return;

                        var descriptions =
                            from sObjs in obj.parent.SubObjects
                            where sObjs.Identifier == linkedTo
                            select sObjs;

                        var fortranObjects = descriptions as IList<FortranObject> ?? descriptions.ToList();

                        if (fortranObjects.Count() > 1)
                        {
                            var curObj = obj;
                            var file = obj.GoUpTillType<SourceFile>();
                            Report.Warning(pub =>
                            {
                                pub.AddWarningDescription("Description meta-data was ignored");
                                pub.AddReason("Description specified multiple times. Using first occurence");
                                pub.AddLocation(curObj.lines.First().Number == curObj.lines.Last().Number
                                    ? $"At line {curObj.lines.First().Number} of '{file.Name}{file.Extension}'."
                                    : $"Within lines {curObj.lines.First().Number} to {curObj.lines.Last().Number} of '{file.Name}{file.Extension}'.");
                            });
                        }

                        var parSubObj = fortranObjects.FirstOrDefault();

                        if (parSubObj != null)
                        {
                            obj.parent.SubObjects.Remove(obj);
                            parSubObj.AddSubObject(obj);
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

        private static void CorrectName(FortranObject obj)
        {
            var desc = (NamedDescription)obj;

            if (obj.parent.Identifier == desc.LinkedTo)
            {
                return;
            }

            var curObj = obj;
            var file = obj.GoUpTillType<SourceFile>();
            Report.Warning(pub =>
            {
                pub.AddWarningDescription("Description meta-data was ignored");
                pub.AddReason("Description identifier does not match parent identifier.");
                pub.AddLocation(curObj.lines.First().Number == curObj.lines.Last().Number
                    ? $"At line {curObj.lines.First().Number} of '{file.Name}{file.Extension}'."
                    : $"Within lines {curObj.lines.First().Number} to {curObj.lines.Last().Number} of '{file.Name}{file.Extension}'.");
            });
            obj.parent.SubObjects.Remove(obj);
        }

        private static void CheckUniqueness(FortranObject obj)
        {
            if (obj.parent.SubObjectsOfType<Description2>().Count <= 1)
            {
                return;
            }

            var curObj = obj;
            var file = obj.GoUpTillType<SourceFile>();
            if (obj.parent is Project2)
            {
                Report.Warning(pub =>
                {
                    pub.AddWarningDescription("Description meta-data was ignored");
                    pub.AddReason("Multiple descriptions specified for a single block.");
                    pub.AddLocation(curObj.lines.First().Number == curObj.lines.Last().Number
                        ? $"At line {curObj.lines.First().Number} of '{file.Name}{file.Extension}'."
                        : $"Within lines {curObj.lines.First().Number} to {curObj.lines.Last().Number} of '{file.Name}{file.Extension}'.");
                });
            }
            else
            {
                Report.Warning(pub =>
                {
                    pub.AddWarningDescription("Description meta-data was ignored");
                    pub.AddReason("Multiple descriptions specified for a single block.");
                    pub.AddLocation(curObj.lines.First().Number == curObj.lines.Last().Number
                        ? $"At line {curObj.lines.First().Number} of '{file.Name}{file.Extension}'."
                        : $"Within lines {curObj.lines.First().Number} to {curObj.lines.Last().Number} of '{file.Name}{file.Extension}'.");
                });
            }
            obj.parent.SubObjects.Remove(obj);
        }
    }
}