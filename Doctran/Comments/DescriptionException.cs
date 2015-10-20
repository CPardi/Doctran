namespace Doctran.Comments
{
    using System.Linq;
    using Parsing;
    using Parsing.FortranObjects;
    using Reporting;

    public class DescriptionException : PostAction
    {
        public DescriptionException()
            : base(typeof(Description))
        { }

        public override void PostObject(ref FortranObject obj)
        {
            this.CorrectName(ref obj);
            this.CheckUniqueness(ref obj);
        }

        public void CorrectName(ref FortranObject obj)
        {
            if (obj.parent.Identifier == obj.Identifier)
            {
                return;
            }

            var curObj = obj;
            var file = obj.GoUpTillType<File>();
            Report.Warning(pub =>
            {
                pub.AddWarningDescription("Description meta-data was ignored");
                pub.AddReason("Description identifier does not match parent identifier.");
                pub.AddLocation(curObj.lines.First().Number == curObj.lines.Last().Number
                    ? $"At line {curObj.lines.First().Number} of '{file.Name}{file.Info.Extension}'."
                    : $"Within lines {curObj.lines.First().Number} to {curObj.lines.Last().Number} of '{file.Name}{file.Info.Extension}'.");
            });
            obj.parent.SubObjects.Remove(obj);
        }

        public void CheckUniqueness(ref FortranObject obj)
        {
            if (obj.parent.SubObjectsOfType<Description>().Count <= 1)
            {
                return;
            }

            var curObj = obj;
            var file = obj.GoUpTillType<File>();
            if (obj.parent is Project)
            {
                Report.Warning(pub =>
                {
                    pub.AddWarningDescription("Description meta-data was ignored");
                    pub.AddReason("Multiple descriptions specified for a single block.");
                    pub.AddLocation(curObj.lines.First().Number == curObj.lines.Last().Number
                        ? $"At line {curObj.lines.First().Number} of '{file.Name}{file.Info.Extension}'."
                        : $"Within lines {curObj.lines.First().Number} to {curObj.lines.Last().Number} of '{file.Name}{file.Info.Extension}'.");
                });
            }
            else
            {
                Report.Warning(pub =>
                {
                    pub.AddWarningDescription("Description meta-data was ignored");
                    pub.AddReason("Multiple descriptions specified for a single block.");
                    pub.AddLocation(curObj.lines.First().Number == curObj.lines.Last().Number
                        ? $"At line {curObj.lines.First().Number} of '{file.Name}{file.Info.Extension}'."
                        : $"Within lines {curObj.lines.First().Number} to {curObj.lines.Last().Number} of '{file.Name}{file.Info.Extension}'.");
                });
            }
            obj.parent.SubObjects.Remove(obj);
        }
    }
}