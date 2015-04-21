//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

using Doctran.Fbase.Files;
using Doctran.Fbase.Common;
using Doctran.Fbase.Comments;

using Doctran.BaseClasses;

namespace Doctran.Exceptions
{
    public class DescriptionException : PostAction
    {
        public DescriptionException()
            : base(typeof(Description))
        { }

        public override void PostObject(ref FortranObject obj)
        {
            this.correctName(ref obj);
            this.checkUniqueness(ref obj);
        }

        public void correctName(ref FortranObject obj)
        {
            if (obj.parent.Identifier != obj.Identifier)
            {
                var file = obj.GoUpTillType<File>();
                Console.WriteLine("----------------------Warning----------------------");
                Console.WriteLine("Error from line '" + obj.lines.First().Number
                                  + "' to '" + obj.lines.Last().Number
                                  + "' of file '" + file.Name + file.Info.Extension + "'.");
                Console.WriteLine("Description identifier does not match parent identifier and has been ignored.");
                obj.parent.SubObjects.Remove(obj);
            }
        }

        public void checkUniqueness(ref FortranObject obj)
        {
            if (obj.parent.SubObjectsOfType<Description>().Count > 1)
            {
                var file = obj.GoUpTillType<File>();
                Console.WriteLine("----------------------Warning----------------------");
                Console.WriteLine("Error from line '" + obj.lines.First().Number
                                  + "' to '" + obj.lines.Last().Number
                                  + "' of file '" + file.Name + file.Info.Extension + "'.");
                Console.WriteLine("Description is not unique and has been ignored.");
                obj.parent.SubObjects.Remove(obj);
            }
        }
    }
}

namespace Doctran.Fbase.Comments
{
    public class DescriptionBlock : FortranBlock
    {
        private InformationBlock infoBlock = new InformationBlock();
        private NamedDescriptionBlock nDescBlock = new NamedDescriptionBlock();

        public DescriptionBlock() 
        {
            this.CheckInternal = false;
            this.Weight = 3;
        }

        public override bool BlockStart(Type parentType, List<FileLine> lines, int lineIndex)
        {
			return lines[lineIndex].Text.Trim().StartsWith("!>")
                && !infoBlock.BlockStart(parentType, lines, lineIndex);
        }

        public override bool BlockEnd(Type parentType, List<FileLine> lines, int lineIndex)
        {
            if(lineIndex + 1 >= lines.Count) return true;
            return
                   !lines[lineIndex + 1].Text.Trim().StartsWith("!>")
                 | infoBlock.BlockStart(parentType, lines, lineIndex + 1)
                 | nDescBlock.BlockStart(parentType, lines, lineIndex + 1);
        }

        public override List<FortranObject> ReturnObject(FortranObject parent, List<FileLine> lines)
        {
            return new List<FortranObject> { new Description(parent, lines) };
        }
    }

    public class DescriptionPostAction : PostAction
    {
        public DescriptionPostAction() : base(typeof(Description)) { }

        public override void PostObject(ref FortranObject obj)
        {
            // If this is a description directly below the definition statement then dont move it. This is really
            // just for function where the result name is the same as the function name.
            if (obj.parent.Identifier == obj.Identifier 
                && obj.parent.lines.Count > 1 && obj.parent.lines[1].Number == obj.lines[0].Number) return;

            String ident = obj.Identifier;
            var temp = (from sObjs in obj.parent.SubObjects
                        where sObjs.Identifier == ident
                        where sObjs.GetType() != typeof(Description)
                        select sObjs);
            FortranObject parSubObj = temp.SingleOrDefault();

            if (parSubObj != null)
            {
                obj.parent.SubObjects.Remove(obj);
                obj.parent = parSubObj;
                parSubObj.SubObjects.Add(obj);
            }
        }
    }

    public class DescriptionGroup : ObjectGroup
    {
        public DescriptionGroup()
            : base(typeof(Description)) { }

        public override XElement XEle(IEnumerable<XElement> Content)
        {
            return Content.SingleOrDefault();
        }
    }

    public class Description : XFortranObject
    {
        public String Basic { get; private set; }
        public String Detailed { get; private set; }

        private String identifier;

        public Description() { }

        public Description(FortranObject parent, String basicText)
            : base(parent, "Description", new List<FileLine> { new FileLine(0, basicText) }, false)
        {
            this.Basic = basicText;
        }

        public Description(FortranObject parent, String identifier, String basicText)
            : base(parent, "Description", new List<FileLine> { new FileLine(0, basicText) }, false)
        {
            this.identifier = identifier;
            this.Basic = basicText;
        }

        public Description(FortranObject parent, String identifier, String basicText, String detailedText, List<FileLine> lines)
            : base(parent, "Description", lines, false)
        {
            this.identifier = identifier;
            this.Basic = basicText;
            this.Detailed = detailedText;
        }

        public Description(FortranObject parent, List<FileLine> lines)
            : base(parent, "Description", lines, false)
        {
            this.Basic = String.Concat(
                from line in lines
                where Regex.IsMatch(line.Text, @"!>\s*\w")
                select (Regex.Match(line.Text, @"!>(.*)").Groups[1].Value.Trim()) + " ").TrimEnd();
            this.Detailed = String.Concat(
                from line in lines
                where Regex.IsMatch(line.Text, @"!>>(.*)")
                select (Regex.Match(line.Text, @"!>>(.*)").Groups[1].Value.Trim()) + " ").TrimEnd();
        }

        protected override String GetIdentifier()
        {
            if (this.identifier == null) return this.parent.Identifier;
            else return this.identifier;
        }

        protected XElement parse(String name, String basic)
        {
            try
            {
                return XElement.Parse("<" + name + ">" + basic + "</" + name + ">", LoadOptions.PreserveWhitespace);
            }
            catch
            {
                var file = this.GoUpTillType<Files.File>();
                Console.WriteLine();
                Console.WriteLine("----------------------Warning----------------------");
                Console.WriteLine("Error from line '" + this.lines.First().Number 
                                  + "' to '" + this.lines.Last().Number 
                                  + "' of file '" + file.Name + file.Info.Extension + "'.");
                Console.WriteLine(name + " description could not be parsed and was ignored.");
                return new XElement(name);
            }
        }

        public override XElement XEle()
        {
            XElement xele = new XElement(this.XElement_Name);
            xele.Add(this.parse("Basic", this.Basic));
            if (this.Detailed != null) xele.Add(this.parse("Detailed", this.Detailed));
            return xele;
        }
    }

}