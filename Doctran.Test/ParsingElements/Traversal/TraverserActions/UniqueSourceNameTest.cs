// <copyright file="UniqueSourceNameTest.cs" company="Christopher Pardi">
//     Copyright © 2015 Christopher Pardi
//     This Source Code Form is subject to the terms of the Mozilla Public
//     License, v. 2.0. If a copy of the MPL was not distributed with this
//     file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

namespace Doctran.Test.ParsingElements.Traversal.TraverserActions
{
    using System;
    using System.Collections.Generic;
    using Doctran.Helper;
    using Doctran.Parsing;
    using Doctran.ParsingElements;
    using Doctran.ParsingElements.FortranObjects;
    using Doctran.ParsingElements.Traversal;
    using NUnit.Framework;

    [TestFixture]
    [Category("Unit")]
    public class UniqueSourceNameTest
    {
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidNotUnique()
        {
            var none = new IContained[] { };
            var empty = new List<FileLine>();
            var sourceList = new List<ISourceFile>
            {
                new SourceFile(string.Empty, "project/src/File1", none, string.Empty, empty),
                new SourceFile(string.Empty, "project/src/File1", none, string.Empty, empty)
            };

            var project = new Project(sourceList);

            ITraverserAction action = TraverserActions.UniqueSourceNames as ITraverserAction<Project>;
            action.Act(project, new StandardErrorListener<TraverserException>());
        }

        [Test]
        public void Unique()
        {
            var none = new IContained[] { };
            var empty = new List<FileLine>();
            var sourceList = new List<ISourceFile>
            {
                new SourceFile(string.Empty, "project/src/File1", none, string.Empty, empty),
                new SourceFile(string.Empty, "project/src/File2", none, string.Empty, empty)
            };

            var project = new Project(sourceList);

            ITraverserAction action = TraverserActions.UniqueSourceNames as ITraverserAction<Project>;
            action.Act(project, new StandardErrorListener<TraverserException>());
            Assert.AreEqual(0, project.SourceNameUniquenessLevel);
            Assert.AreEqual("File1", sourceList[0].Name);
            Assert.AreEqual("File2", sourceList[1].Name);
        }

        [Test]
        public void ValidNotUnique()
        {
            var none = new IContained[] { };
            var empty = new List<FileLine>();
            var sourceList = new List<ISourceFile>
            {
                new SourceFile(string.Empty, "project/src/File1", none, string.Empty, empty),
                new SourceFile(string.Empty, "project/examples/File1", none, string.Empty, empty)
            };

            var project = new Project(sourceList);

            ITraverserAction action = TraverserActions.UniqueSourceNames as ITraverserAction<Project>;
            action.Act(project, new StandardErrorListener<TraverserException>());
            Assert.AreEqual(1, project.SourceNameUniquenessLevel);
            Assert.AreEqual("src/File1", sourceList[0].Name);
            Assert.AreEqual("examples/File1", sourceList[1].Name);
        }
    }
}