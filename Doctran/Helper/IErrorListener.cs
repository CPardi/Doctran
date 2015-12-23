namespace Doctran.Helper
{
    using System;

    public interface IErrorListener<TException>
        where TException : Exception
    {
        ReportException<TException> Error { get; }

        ReportException<TException> Warning { get; }
    }
}