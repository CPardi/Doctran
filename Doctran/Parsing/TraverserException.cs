namespace Doctran.Parsing
{
    using System;

    public class TraverserException : ApplicationException
    {
        public TraverserException(IFortranObject fortranObject, string message)
            : base(message)
        {
            this.FortranObject = fortranObject;
        }

        public IFortranObject FortranObject { get; }
    }
}