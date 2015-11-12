namespace Doctran.Parsing
{
    using System;

    public interface ITraverserAction
    {
        Action<object> Act { get; }
        Type ForType { get; }
    }

    public interface ITraverserAction<in T> : ITraverserAction
    {
        new Action<T> Act { get; }
    }
}