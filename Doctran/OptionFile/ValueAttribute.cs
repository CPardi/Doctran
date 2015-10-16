using System;

namespace Doctran.OptionFile
{
    public class ValueAttribute : Attribute
    {
        public ValueAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }    
    }
}
