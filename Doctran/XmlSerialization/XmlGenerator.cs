// <copyright file="XmlGenerator.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.XmlSerialization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Parsing;
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
                from groupType in _toGroupXmlDictionary.Keys
                where !_toXmlDictionary.Keys.Any(objectType => IsParentOrThis(groupType, objectType))
                select groupType;

            var str = string.Concat(keys.Select((k, i) => i == 0 ? $"'{k.Name}'" : $", '{k.Name}'"));
            if (str != string.Empty)
            {
                throw new ApplicationException($"A group XElement exists for the types {str} for which there is no corresponding object XElement.");
            }
        }

        public XElement CreateForObject(IFortranObject obj) => this.GetXmlValue(new[] { obj }).Single();

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

        private IEnumerable<XElement> GetValue(Type groupType, IEnumerable<IFortranObject> objsOfType)
        {
            Func<IEnumerable<XElement>, XElement> toGroupXml;
            _toGroupXmlDictionary.TryGetValue(groupType, out toGroupXml);

            var objsOfTypeList = objsOfType as IList<IFortranObject> ?? objsOfType.ToList();

            var objHasXml =
                (from obj in objsOfTypeList
                    let objType = obj.GetType()
                    where _toXmlDictionary.ContainsKey(objType) || objType.GetInterfaces().Any(inter => _toXmlDictionary.ContainsKey(inter))
                    select objsOfTypeList).Any();

            return objHasXml
                ? toGroupXml != null
                    ? CollectionUtils.Singlet(toGroupXml(this.GetXmlValue(objsOfTypeList)))
                    : this.GetXmlValue(objsOfTypeList)
                : this.SkipLevel(objsOfTypeList);
        }

        private IEnumerable<XElement> GetXmlValue(IEnumerable<IFortranObject> objsOfType)
        {
            var xElements = new List<XElement>();
            foreach (var obj in objsOfType)
            {
                var objType = obj.GetType();
                Func<IFortranObject, XElement> toXml;
                _toXmlDictionary.TryGetValue(objType, out toXml);
                if (toXml == null)
                {
                    continue;
                }

                var xElement = toXml(obj);
                xElement.Add(objType.GetInterfaces().Select(inter =>
                {
                    Func<IFortranObject, IEnumerable<XElement>> create;
                    _interfaceXmlDictionary.TryGetValue(inter, out create);
                    return create != null ? create(obj) : CollectionUtils.Empty<XElement>();
                }));

                xElement.Add(this.Navigate(obj));
                xElements.Add(xElement);
            }

            return xElements.Where(xElement => xElement != null);
        }

        private Type KeySelector(IFortranObject obj)
        {
            var objType = obj.GetType();
            return objType.GetInterfaces().SingleOrDefault(inter => _toGroupXmlDictionary.ContainsKey(inter)) ?? objType;
        }

        private IEnumerable<XElement> Navigate(IFortranObject obj)
            => (obj as IContainer)?.SubObjects.GroupBy(this.KeySelector).SelectMany(objsOfType => this.GetValue(objsOfType.Key, objsOfType)) ?? new XElement[] { };

        private IEnumerable<XElement> SkipLevel(IEnumerable<IFortranObject> objsOfType) => objsOfType.SelectMany(this.Navigate);
    }
}