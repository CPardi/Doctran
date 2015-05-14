//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;

using Doctran;
using Doctran.Fbase.Common;
using Doctran.BaseClasses;

using Doctran.Fbase.Files;
using Doctran.Fbase.Comments;

namespace Doctran.Fbase.Projects
{
    public class Project : XFortranObject
    {
        #region Private fields

        private DateTime _creationDate = DateTime.Now;

        #endregion

        #region Constructor

        public Project(Common.Settings settings)
        {
            // Read the project file if there is one.
            if (settings.has_info)
            {
                try
                {
                    this.lines =
                        (from line in File.ReadFile(System.IO.Path.GetFullPath(settings.ProjectInfo))
                         where line.Text != ""
                         select new FileLine(line.Number, "!>" + line.Text)
                        ).ToList();
                }
                catch (System.IO.IOException e) { UserInformer.GiveError("project info", settings.ProjectInfo, e); }

                this.Search();
            }

            this.Name = this.GetName();
            this.GetMarkupType();
            this.AddDefaults();

            try
            {
                this.SubObjects.AddRange(
                    from path in settings.SourceFiles.AsParallel()
                    where System.IO.File.Exists(path)
                    let fullpath = System.IO.Path.GetFullPath(path)
                    select new File(this, fullpath, File.ReadFile(fullpath))
                );
            }
            catch (System.IO.IOException e) { UserInformer.GiveError("source file", settings.ProjectInfo, e); }
        }

        #endregion

        #region Public Properties

        public bool UseText { get; private set; }
        public bool UseHtml { get; private set; }
        public bool UseMd { get; private set; }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get the name of the project from project file.
        /// </summary>
        /// <returns></returns>
        private String GetName()
        {
            var nameInfo = this.SubObjectsOfType<Information>().Where(info => info.Type == "Name");
            switch (nameInfo.Count())
            {
                case 0:
                    return "My Project";
                case 1:
                    this.SubObjects.RemoveAll(info => info == nameInfo);
                    return nameInfo.Single().Value;
                default:
                    UserInformer.GiveWarning("project info", "Name specified more than once. Using first occurence");
                    this.SubObjects.RemoveAll(info => info == nameInfo);
                    return nameInfo.First().Value;
            }
        }

        /// <summary>
        /// Get the markup type specified in the project file.
        /// </summary>
        private void GetMarkupType()
        {
            var markup = "markdown";
            try
            {
                var markupInfo = this.SubObjectsOfType<Information>().Where(info => info.Type == "Markup").SingleOrDefault();
                if (markupInfo != null) markup = markupInfo.Value;
            }
            catch (InvalidOperationException)
            {
                UserInformer.GiveWarning("project info", "Markup type specified multiple times, using Markdown");
            }

            switch (markup.ToLower())
            {
                case "text":
                    this.UseText = true;
                    break;
                case "html":
                    this.UseHtml = true;
                    break;
                case "markdown":
                    this.UseMd = true;
                    break;
                default:
                    UserInformer.GiveWarning("project info", "Specified markup type is invalid, using Markdown");
                    break;
            }
        }

        private void AddDefaults()
        {
            this.AddDefault(new Information(this, "ShowSource", "", new List<SubInformation>() { 
                new SubInformation(this, "Type", "Function"),
                new SubInformation(this, "Type", "Subroutine")
            }));

            this.AddDefault(new Information(this, "Searchable", "", new List<SubInformation>() { 
                new SubInformation(this, "Type", "File"),
                new SubInformation(this, "Type", "Module"),
                new SubInformation(this, "Type", "Type"),
                new SubInformation(this, "Type", "Assignment"),
                new SubInformation(this, "Type", "Operator"),
                new SubInformation(this, "Type", "Overload"),
                new SubInformation(this, "Type", "Function"),
                new SubInformation(this, "Type", "Subroutine")
            }));
        }

        /// <summary>
        /// Add a default in case required information is not specified in the project file.
        /// </summary>
        /// <param name="defaultInfo">The information that should be used if some is not already specified.</param>
        private void AddDefault(Information defaultInfo)
        {
            var temp = this.SubObjectsOfType<Information>().Where(info => info.Type == defaultInfo.Type);
            if (!temp.Any())
                this.SubObjects.Add(defaultInfo);
        }

        #endregion

        #region Overrides

        protected override String GetIdentifier()
        {
            return this.Name;
        }

        public override XElement XEle()
        {
            XElement xele = new XElement("Project");

            xele.Add(new XElement("Name", this.Name));
            xele.Add(new XElement("DocCreated", this._creationDate.XEle()));
            xele.Add(
                new InformationGroup().XEle(
                from info in this.SubObjectsOfType<Information>()
                select info.XEle()
                    ));
            xele.Add(
                from info in this.SubObjectsOfType<Description>()
                select info.XEle()
                    );
            xele.Add(new XElement("Files",
                from file in this.SubObjectsOfType<File>()
                select file.XEle())
                    );
            return xele;
        }

        #endregion
    }
}
