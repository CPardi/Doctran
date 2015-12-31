namespace Doctran.Parsing
{
    using System;

    public class TraverserException
        : ApplicationException
    {
        public TraverserException(object cause, string message)
            : base(message)
        {
            this.Cause = cause;
        }

        public object Cause { get; }
    }
}