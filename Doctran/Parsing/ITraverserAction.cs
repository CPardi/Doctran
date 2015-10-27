using System;

namespace Doctran.Parsing
{
    using System.Collections;
    using System.Collections.Generic;

    public interface ITraverserAction
    {
        Type ForType { get; }

        Action<object> Act { get; }
    }

    public interface ITraverserAction<in T> : ITraverserAction
    {
        new Action<T> Act { get; }
    }
}