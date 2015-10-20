namespace Doctran.Parsing.PostActions
{
    using System.Collections.Generic;
    using System.Linq;
    using FortranObjects;
    using Parsing;
    using Reporting;

    public class DescriptionPostAction : PostAction
    {
        public DescriptionPostAction() : base(typeof(Description)) { }

        public override void PostObject(ref FortranObject obj)
        {
            // If this is a description directly below the definition statement then dont move it. This is really
            // just for function where the result name is the same as the function name.
            if (obj.parent.Identifier == obj.Identifier 
                && obj.parent.lines.Count > 1 && obj.parent.lines[1].Number == obj.lines[0].Number) return;

            string ident = obj.Identifier;
            var descriptions = (from sObjs in obj.parent.SubObjects
                where sObjs.Identifier == ident
                where sObjs.GetType() != typeof(Description)
                select sObjs);

            var fortranObjects = descriptions as IList<FortranObject> ?? descriptions.ToList();

            if (fortranObjects.Count() > 1)
            {
                var curObj = obj;
                var file = obj.GoUpTillType<File>();
                Report.Warning(pub =>
                {
                    pub.AddWarningDescription("Description meta-data was ignored");
                    pub.AddReason("Description specified multiple times. Using first occurence");
                    pub.AddLocation(curObj.lines.First().Number == curObj.lines.Last().Number
                        ? $"At line {curObj.lines.First().Number} of '{file.Name}{file.Info.Extension}'."
                        : $"Within lines {curObj.lines.First().Number} to {curObj.lines.Last().Number} of '{file.Name}{file.Info.Extension}'.");
                });
            }

            FortranObject parSubObj = fortranObjects.FirstOrDefault();

            if (parSubObj != null)
            {
                obj.parent.SubObjects.Remove(obj);
                parSubObj.AddSubObject(obj);
            }
        }
    }
}