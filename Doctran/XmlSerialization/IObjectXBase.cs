namespace Doctran.XmlSerialization
{
    using System;

    public interface IObjectXBase
    {
        Type ForType { get; }

        XmlTraversalType XmlTraversalType { get; }
    }
}