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
    using Helper;
    using Parsing;
    using Utilitys;

    public class XmlGenerator
    {
        private readonly Dictionary<Type, IInterfaceXElements> _interfaceXmlDictionary;

        private readonly Dictionary<Type, Func<IEnumerable<XElement>, XElement>> _toGroupXmlDictionary;

        private readonly Dictionary<XmlGeneratorKey, IObjectXBase> _toXmlDictionary;

        public XmlGenerator(
            IEnumerable<IInterfaceXElements> interfaceXElements,
            IEnumerable<IObjectXBase> objectXElements,
            IEnumerable<IGroupXElement> toGroupXElements)
        {
            _interfaceXmlDictionary = interfaceXElements.ToDictionary(obj => obj.ForType);

            var objectXElementsList = objectXElements as IObjectXElement[] ?? objectXElements.ToArray();
            _toXmlDictionary = objectXElementsList.ToDictionary(
                obj => new XmlGeneratorKey(obj.ForType, obj.XmlTraversalType),
                new KeyComparer(objectXElementsList.Select(oxe => oxe.ForType)));

            var groupXElementsArray = toGroupXElements as IGroupXElement[] ?? toGroupXElements.ToArray();
            _toGroupXmlDictionary = groupXElementsArray.ToDictionary(
                obj => obj.ForType,
                obj => new Func<IEnumerable<XElement>, XElement>(obj.Create),
                new CompareRootTypes(groupXElementsArray.Select(oxe => oxe.ForType)));

            var keys =
                from groupType in _toGroupXmlDictionary.Keys
                where !_toXmlDictionary.Keys.Any(objectType => IsParentOrThis(groupType, objectType.Type))
                select groupType;

            var str = string.Concat(keys.Select((k, i) => i == 0 ? $"'{k.Name}'" : $", '{k.Name}'"));
            if (str != string.Empty)
            {
                throw new ApplicationException($"A group XElement exists for the types {str} for which there is no corresponding object XElement.");
            }
        }

        public XElement CreateForObject(IFortranObject obj) => this.GetXmlValue(new[] { obj }, XmlTraversalType.HeadOrSubObjects).Single();

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

        private IEnumerable<XElement> GetValue(Type groupType, IEnumerable<IFortranObject> objsOfType, XmlTraversalType xmlTraversalType)
        {
            Func<IEnumerable<XElement>, XElement> toGroupXml;
            _toGroupXmlDictionary.TryGetValue(groupType, out toGroupXml);

            var objsOfTypeList = objsOfType as IList<IFortranObject> ?? objsOfType.ToList();

            var objHasXml =
                (from obj in objsOfTypeList
                    let objType = obj.GetType()
                    where _toXmlDictionary.ContainsKey(new XmlGeneratorKey(objType, xmlTraversalType))
                          || objType.GetInterfaces().Any(inter => _toXmlDictionary.ContainsKey(new XmlGeneratorKey(inter, xmlTraversalType)))
                    select objsOfTypeList).Any();

            IEnumerable<XElement> xmlValue = this.GetXmlValue(objsOfTypeList, xmlTraversalType)?.ToList();
            return objHasXml
                ? toGroupXml != null && xmlValue != null && xmlValue.Any()
                    ? CollectionUtils.Singlet(toGroupXml(xmlValue))
                    : xmlValue
                : CollectionUtils.Empty<XElement>();
            //: this.SkipLevel(objsOfTypeList);
        }

        private IEnumerable<XElement> GetXmlValue(IEnumerable<IFortranObject> objsOfType, XmlTraversalType xmlTraversalType)
        {
            var xElements = new List<XElement>();
            foreach (var obj in objsOfType)
            {
                var objType = obj.GetType();

                IObjectXBase objectXBase;

                if (!_toXmlDictionary.TryGetValue(new XmlGeneratorKey(objType, xmlTraversalType), out objectXBase))
                {
                    continue;
                }

                var objectXElement = objectXBase as IObjectXElement;

                if (objectXElement == null)
                {
                    xElements.AddRange(this.Navigate(obj));
                    continue;
                }

                var xElement = objectXElement.Create(obj);
                if (objectXElement.GetXmlCreationType(obj) == XmlCreationType.All)
                {
                    xElement?.Add(objType.GetInterfaces().Select(inter =>
                    {
                        // Try to get the interface XElement creator. If it exists and instructed by it to create the XElements, then do it.
                        IInterfaceXElements interfaceXElements;
                        _interfaceXmlDictionary.TryGetValue(inter, out interfaceXElements);
                        IEnumerable<XObject> xObjectsFromInterface;
                        return interfaceXElements != null && interfaceXElements.ShouldCreate(obj) && (xObjectsFromInterface = interfaceXElements.Create(obj)) != null
                            ? xObjectsFromInterface
                            : CollectionUtils.Empty<XElement>();
                    }));
                    xElement?.Add(this.Navigate(obj));
                }

                xElements.Add(xElement);
            }

            return xElements.Where(xElement => xElement != null);
        }

        private IEnumerable<XElement> Navigate(IFortranObject obj)
        {
            var xElementList = new List<XElement>();
            xElementList.AddRange(
                (obj as IContainer)
                    ?.SubObjects
                    .GroupBy(obj1 => obj1.GetType())
                    .SelectMany(objsOfType => this.GetValue(objsOfType.Key, objsOfType, XmlTraversalType.HeadOrSubObjects))
                ?? new XElement[] { });

            xElementList.AddRange(
                (obj as IQuasiContainer)
                    ?.QuasiObjects
                    .GroupBy(obj1 => obj1.GetType())
                    .SelectMany(objsOfType => this.GetValue(objsOfType.Key, objsOfType, XmlTraversalType.QuasiObjects))
                ?? new XElement[] { });

            // Combine any elements with the same name.
            return xElementList
                .GroupBy(nodes => nodes.Name)
                .Select(group => new XElement(group.Key, group.SelectMany(item => item.Nodes())));
        }

        private IEnumerable<XElement> SkipLevel(IEnumerable<IFortranObject> objsOfType) => objsOfType.SelectMany(this.Navigate);
    }
}