namespace Doctran.ParsingElements
{
    using System;
    using System.Collections.Generic;

    public interface IErrorProne
    {
        void ForceValidity(out IList<Exception> fixedErrors);
    }
}