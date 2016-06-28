namespace Doctran.XmlSerialization
{
    using System;
    using Parsing;

    public class ObjectXPassthrough<TParsed> : IObjectXBase
        where TParsed : IFortranObject
    {
        public ObjectXPassthrough(XmlTraversalType xmlTraversalType = XmlTraversalType.HeadOrSubObjects)
        {
            this.XmlTraversalType = xmlTraversalType;
        }

        public Type ForType => typeof(TParsed);

        public XmlTraversalType XmlTraversalType { get; }
    }
}