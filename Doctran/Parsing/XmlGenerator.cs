namespace Doctran.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Utilitys;

    public class XmlGenerator
    {
        private readonly Dictionary<Type, Func<FortranObject, IEnumerable<XElement>>> _interfaceXmlDictionary;
        private readonly Dictionary<Type, Func<IEnumerable<XElement>, XElement>> _toGroupXmlDictionary;
        private readonly Dictionary<Type, Func<FortranObject, XElement>> _toXmlDictionary;

        public XmlGenerator(
            IEnumerable<IInterfaceXElements> interfaceXElements,
            IEnumerable<IObjectXElement> objectXElements,
            IEnumerable<IGroupXElement> toGroupXmlDictionary)
        {
            _interfaceXmlDictionary = interfaceXElements.ToDictionary(
                obj => obj.ForType,
                obj => new Func<FortranObject, IEnumerable<XElement>>(obj.Create));

            _toXmlDictionary = objectXElements.ToDictionary(
                obj => obj.ForType,
                obj => new Func<FortranObject, XElement>(obj.Create));

            _toGroupXmlDictionary = toGroupXmlDictionary.ToDictionary(
                obj => obj.ForType,
                obj => new Func<IEnumerable<XElement>, XElement>(obj.Create));

            var keys =
                from type in this._toGroupXmlDictionary.Keys
                where !this._toXmlDictionary.Keys.Contains(type)
                select type;

            var str = string.Concat(keys.Select((k, i) => i == 0 ? $"'{k.Name}'" : $", '{k.Name}'"));
            if (str != string.Empty)
            {
                throw new ApplicationException($"A group XElement exists for the types {str} for which there is no corresponding object XElement.");
            }
        }

        public IEnumerable<XElement> CreateForObject(FortranObject sourceFile)
        {
            return GetValue(sourceFile.GetType(), HelperUtils.Singlet(sourceFile));
        }

        private IEnumerable<XElement> GetValue(Type objType, IEnumerable<FortranObject> objsOfType)
        {
            Func<FortranObject, XElement> toXml;
            this._toXmlDictionary.TryGetValue(objType, out toXml);

            Func<IEnumerable<XElement>, XElement> toGroupXml;
            this._toGroupXmlDictionary.TryGetValue(objType, out toGroupXml);

            if (toGroupXml == null && toXml != null)
            {
                return this.GetXmlValue(objType, objsOfType, toXml);
            }

            if (toGroupXml != null && toXml != null)
            {
                return HelperUtils.Singlet(toGroupXml(this.GetXmlValue(objType, objsOfType, toXml)));
            }

            return this.SkipLevel(objsOfType);
        }

        private IEnumerable<XElement> GetXmlValue(Type objType, IEnumerable<FortranObject> objsOfType, Func<FortranObject, XElement> toXml)
        {
            var xElements = new List<XElement>();
            foreach (var fortranObject in objsOfType)
            {
                var xElement = toXml(fortranObject);
                xElement.Add(objType.GetInterfaces().Select(inter => _interfaceXmlDictionary[inter](fortranObject)));
                xElement.Add(this.Navigate(fortranObject));
                xElements.Add(xElement);
            }
            return xElements.Where(xElement => xElement != null);
        }

        private IEnumerable<XElement> Navigate(FortranObject obj)
        {
            var xElements = new List<XElement>();
            var subObjects = obj.SubObjects;

            xElements.AddRange(
                from objsOfType in subObjects.GroupBy(sObj => sObj.GetType())
                from xElement in this.GetValue(objsOfType.Key, objsOfType)
                select xElement);

            return xElements;
        }

        private IEnumerable<XElement> SkipLevel(IEnumerable<FortranObject> objsOfType) => objsOfType.SelectMany(this.Navigate);
    }
}