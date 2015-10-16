using System;

namespace Doctran.OptionFile
{
    public class XmlPassThroughOptionsAttribute : Attribute
    {
        public XmlPassThroughOptionsAttribute(string rootName)
        {
            this.RootName = rootName;
        }

        public string RootName { get; private set; }
    }
}
