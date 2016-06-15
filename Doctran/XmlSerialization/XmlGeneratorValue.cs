namespace Doctran.XmlSerialization
{
    using System;
    using System.Xml.Linq;
    using Parsing;

    public class XmlGeneratorValue
    {
        private readonly Func<IFortranObject, XElement> _create;

        private readonly Func<IFortranObject, XmlCreationType> _getXmlCreationType;

        public XmlGeneratorValue(Func<IFortranObject, XmlCreationType> getXmlCreationType, Func<IFortranObject, XElement> create)
        {
            _getXmlCreationType = getXmlCreationType;
            _create = create;
        }

        public XElement Create(IFortranObject obj) => _create(obj);

        public XmlCreationType GetXmlCreationType(IFortranObject obj) => _getXmlCreationType(obj);
    }
}