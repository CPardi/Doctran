namespace Doctran.Input.OptionFile
{
    using System;

    public class ValueAttribute : Attribute
    {
        public ValueAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }    
    }
}
