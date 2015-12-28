namespace Doctran.Helper
{
    using System;

    public interface IErrorListener<in TException>
        where TException : Exception
    {
        /// <summary>
        ///     Report an error to the listener.
        /// </summary>
        void Error(TException exception);

        /// <summary>
        ///     Report an warning to the listener.
        /// </summary>
        void Warning(TException exception);
    }
}