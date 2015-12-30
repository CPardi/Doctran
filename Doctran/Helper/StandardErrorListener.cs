namespace Doctran.Helper
{
    using System;

    public class StandardErrorListener<TException> : IErrorListener<TException>
        where TException : Exception
    {
        public void Error(TException exception)
        {
            throw exception;
        }

        public void Warning(TException exception)
        {
            throw exception;
        }
    }
}