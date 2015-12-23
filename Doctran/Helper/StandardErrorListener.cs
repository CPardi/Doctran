using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doctran.Helper
{
    public class StandardErrorListener<TException> : IErrorListener<TException>
        where TException : Exception
    {
        ReportException<TException> IErrorListener<TException>.Error
        {
            get { return ex => { throw ex; }; }
        }

        ReportException<TException> IErrorListener<TException>.Warning
        {
            get { return ex => { throw ex; }; }
        }
    }
}
