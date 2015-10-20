//  Copyright © 2015 Christopher Pardi
//  This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Doctran.Output
{
    using System.IO;
    using System.Xml.Linq;
    using Utilitys;

    public class XmlOutputter
    {
        public XmlOutputter(string relativePathAndName)
        {
            this.XDocument = XDocument.Load(Path.GetFullPath(relativePathAndName));
        }

        public XmlOutputter(XElement documentBody)
        {
            this.XDocument = new XDocument(
                  new XDeclaration("1.0", "utf-8", "yes"),
                  documentBody);
        }

        public XDocument XDocument { get; set; }

        public void SaveToDisk(string relativePathAndName)
        {
             HelperUtils.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(relativePathAndName)));
             this.XDocument.Save(Path.GetFullPath(relativePathAndName));
        }
    }
}
