namespace Doctran.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    ///     Warnings and errors reported to this <see cref="IErrorListener{TException}" /> are stored and can be accessed as a
    ///     list at a later time.
    /// </summary>
    /// <typeparam name="TException">The type of exception to be listened for.</typeparam>
    public class ListenerAndAggregater<TException> : IErrorListener<TException>
        where TException : Exception
    {
        private readonly List<TException> _errors = new List<TException>();

        private readonly List<TException> _warnings = new List<TException>();

        /// <summary>
        ///     The collection of errors reported to this listener.
        /// </summary>
        public ReadOnlyCollection<TException> Errors => _errors.AsReadOnly();

        /// <summary>
        ///     The collections of warnings reported to this listener.
        /// </summary>
        public ReadOnlyCollection<TException> Warnings => _warnings.AsReadOnly();

        public void Error(TException exception)
        {
            _errors.Add(exception);
        }

        public void Warning(TException exception)
        {
            _warnings.Add(exception);
        }
    }
}