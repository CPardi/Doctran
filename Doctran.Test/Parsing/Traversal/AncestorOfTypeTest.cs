// <copyright file="AncestorOfTypeTest.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Test.Parsing.Traversal
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;
    using Doctran.Helper;
    using Doctran.Parsing;
    using Doctran.ParsingElements;
    using Doctran.ParsingElements.FortranObjects;
    using Doctran.ParsingElements.Scope;
    using NUnit.Framework;

    [TestFixture]
    public class AncestorOfTypeTest
    {
        private readonly IEnumerable<ScopeCalculator> _globalScope = new ScopeCalculator[] { };

        [Test]
        public void NoAncestor()
        {
            var desc = new Description(new XElement("Basic"), new XElement("Detail"), new List<FileLine>());
            var file1 = new SourceFile(string.Empty, @"C:\", new[] { desc }, string.Empty, new List<FileLine>());
            var project = new Project(new[] { file1 }, _globalScope);

            Assert.AreEqual(null, desc.SelfOrAncestorOfType<InformationValue>());
        }

        [Test]
        public void OneDeepChildAncestor()
        {
            var desc = new Description(new XElement("Basic"), new XElement("Detail"), new List<FileLine>());
            var file1 = new SourceFile(string.Empty, @"C:\", new[] { desc }, string.Empty, new List<FileLine>());
            var project = new Project(new[] { file1 }, _globalScope);

            Assert.AreEqual(file1, desc.SelfOrAncestorOfType<SourceFile>());
        }

        [Test]
        public void OneDeepRootAncestor()
        {
            var file1 = new SourceFile(string.Empty, @"C:\", new IContained[] { }, string.Empty, new List<FileLine>());
            var project = new Project(new[] { file1 }, _globalScope);

            Assert.AreEqual(project, file1.SelfOrAncestorOfType<Project>());
        }

        [Test]
        public void TwoDeepRootAncestor()
        {
            var desc = new Description(new XElement("Basic"), new XElement("Detail"), new List<FileLine>());
            var file1 = new SourceFile(string.Empty, @"C:\", new[] { desc }, string.Empty, new List<FileLine>());
            var project = new Project(new[] { file1 }, _globalScope);

            Assert.AreEqual(project, desc.SelfOrAncestorOfType<Project>());
        }
    }
}