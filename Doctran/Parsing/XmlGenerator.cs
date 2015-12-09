namespace Doctran.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Utilitys;

    public class XmlGenerator
    {
        private readonly Dictionary<Type, Func<IFortranObject, IEnumerable<XElement>>> _interfaceXmlDictionary;
        private readonly Dictionary<Type, Func<IEnumerable<XElement>, XElement>> _toGroupXmlDictionary;
        private readonly Dictionary<Type, Func<IFortranObject, XElement>> _toXmlDictionary;

        public XmlGenerator(
            IEnumerable<IInterfaceXElements> interfaceXElements,
            IEnumerable<IObjectXElement> objectXElements,
            IEnumerable<IGroupXElement> toGroupXmlDictionary)
        {
            _interfaceXmlDictionary = interfaceXElements.ToDictionary(
                obj => obj.ForType,
                obj => new Func<IFortranObject, IEnumerable<XElement>>(obj.Create));

            _toXmlDictionary = objectXElements.ToDictionary(
                obj => obj.ForType,
                obj => new Func<IFortranObject, XElement>(obj.Create));

            _toGroupXmlDictionary = toGroupXmlDictionary.ToDictionary(
                obj => obj.ForType,
                obj => new Func<IEnumerable<XElement>, XElement>(obj.Create));

            var keys =
                from groupType in this._toGroupXmlDictionary.Keys
                where !this._toXmlDictionary.Keys.Any(objectType => IsParentOrThis(groupType, objectType))
                select groupType;

            var str = string.Concat(keys.Select((k, i) => i == 0 ? $"'{k.Name}'" : $", '{k.Name}'"));
            if (str != string.Empty)
            {
                throw new ApplicationException($"A group XElement exists for the types {str} for which there is no corresponding object XElement.");
            }
        }

        public IEnumerable<XElement> CreateForObject(IFortranObject sourceFile)
        {
            return GetValue(sourceFile.GetType(), CollectionUtils.Singlet(sourceFile));
        }

        private static bool IsParentOrThis(Type ofThisType, Type type)
        {
            if (type == ofThisType)
            {
                return true;
            }

            if (type.BaseType == null)
            {
                return false;
            }

            return
                IsParentOrThis(ofThisType, type.BaseType)
                || type.GetInterfaces().Count(i => IsParentOrThis(ofThisType, i)) > 0;
        }

        private IEnumerable<XElement> GetValue(Type objType, IEnumerable<IFortranObject> objsOfType)
        {
            Func<IFortranObject, XElement> toXml;
            this._toXmlDictionary.TryGetValue(objType, out toXml);

            Func<IEnumerable<XElement>, XElement> toGroupXml;
            this._toGroupXmlDictionary.TryGetValue(objType, out toGroupXml);

            if (toGroupXml == null && toXml != null)
            {
                return this.GetXmlValue(objType, objsOfType, toXml);
            }

            if (toGroupXml != null && toXml != null)
            {
                return CollectionUtils.Singlet(toGroupXml(this.GetXmlValue(objType, objsOfType, toXml)));
            }

            return this.SkipLevel(objsOfType);
        }

        private IEnumerable<XElement> GetXmlValue(Type objType, IEnumerable<IFortranObject> objsOfType, Func<IFortranObject, XElement> toXml)
        {
            var xElements = new List<XElement>();
            foreach (var fortranObject in objsOfType)
            {
                var xElement = toXml(fortranObject);
                xElement.Add(objType.GetInterfaces().Select(inter =>
                {
                    Func<IFortranObject, IEnumerable<XElement>> create;
                    _interfaceXmlDictionary.TryGetValue(inter, out create);
                    return create != null ? create(fortranObject) : CollectionUtils.Empty<XElement>();
                }));
                xElement.Add(this.Navigate(fortranObject));
                xElements.Add(xElement);
            }

            return xElements.Where(xElement => xElement != null);
        }

        private IEnumerable<XElement> Navigate(IFortranObject obj)
        {
            var xElements = new List<XElement>();
            var subObjects = obj.SubObjects;

            xElements.AddRange(
                from objsOfType in subObjects.GroupBy(sObj => sObj.GetType())
                from xElement in this.GetValue(objsOfType.Key, objsOfType)
                select xElement);

            return xElements;
        }

        private IEnumerable<XElement> SkipLevel(IEnumerable<IFortranObject> objsOfType) => objsOfType.SelectMany(this.Navigate);
    }
}