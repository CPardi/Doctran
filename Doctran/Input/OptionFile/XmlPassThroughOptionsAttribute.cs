namespace Doctran.Input.OptionFile
{
    using System;

    public class XmlPassThroughOptionsAttribute : Attribute
    {
        public XmlPassThroughOptionsAttribute(string rootName)
        {
            this.RootName = rootName;
        }

        public string RootName { get; private set; }
    }
}
