namespace Doctran.Helper
{
    using System;

    public class ErrorListener<TException> : IErrorListener<TException>
        where TException : Exception
    {
        private readonly Action<TException> _error;

        private readonly Action<TException> _warning;

        public ErrorListener(Action<TException> warning, Action<TException> error)
        {
            _warning = warning;
            _error = error;
        }

        public void Error(TException exception)
        {
            _error(exception);
        }

        public void Warning(TException exception)
        {
            _warning(exception);
        }
    }
}