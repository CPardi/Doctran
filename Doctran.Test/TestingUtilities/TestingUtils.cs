namespace Doctran.Test.TestingUtilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Doctran.Parsing;
    using Doctran.ParsingElements.FortranObjects;
    using Doctran.Plugins;
    using Doctran.Utilitys;
    using NUnit.Framework;

    public class TestingUtils
    {
        public static void BlockEndCheck(IFortranBlock block, IEnumerable<IFortranBlock> ancestors, string linesString)
        {
            var lines = Doctran.Utilitys.StringUtils.ConvertToFileLineList(linesString);
            Assert.IsTrue(block.BlockEnd(ancestors, lines, lines.Count - 1), "Without random case.");

            var randomCaseLines = Doctran.Utilitys.StringUtils.ConvertToFileLineList(linesString.ToRandomCase());
            Assert.IsTrue(block.BlockEnd(ancestors, randomCaseLines, lines.Count - 1), "With random case.");
        }

        public static void BlockStartCheck(IFortranBlock block, IEnumerable<IFortranBlock> ancestors, string linesString)
        {
            var lines = Doctran.Utilitys.StringUtils.ConvertToFileLineList(linesString);
            Assert.IsTrue(block.BlockStart(ancestors, lines, 0));

            var randomCaseLines = Doctran.Utilitys.StringUtils.ConvertToFileLineList(linesString.ToRandomCase());
            Assert.IsTrue(block.BlockStart(ancestors, randomCaseLines, 0));
        }

        public static Project CreateDoctranProject(string baseNamespace, ILanguageParser language, Assembly executingAssembly, IEnumerable<string> fileResources)
        {
            var files = fileResources.AsParallel();

            var parsedFiles = files.Select(path =>
            {
                // Parse source files.
                var resourceName = $"{baseNamespace}.{path}";
                var manifestResourceStream = executingAssembly.GetManifestResourceStream(resourceName);
                if (manifestResourceStream == null)
                {
                    throw new InvalidOperationException($"Could not load resource '{resourceName}'");
                }

                var source = new StreamReader(manifestResourceStream).ReadToEnd();
                return language.Parse(path, source);
            }).ToList();

            var project = new Project(parsedFiles, new[] { language.GlobalScopeFactory });
            new Traverser("Global post processing", language.GlobalTraverserActions.ToArray()).Go(project);
            return project;
        }

        public static void ReturnObjectCheck(IFortranBlock block, string linesString, Action<IEnumerable<IFortranObject>> makeAssertions, TestOptions testCaseInvariance = TestOptions.None)
        {
            {
                var lines = Doctran.Utilitys.StringUtils.ConvertToFileLineList(linesString);
                var actual = block.ReturnObject(new IContained[] { }, lines)
                    .ToList<IFortranObject>();
                makeAssertions(actual);
            }

            if (testCaseInvariance != TestOptions.ShouldBeCaseSensitive)
            {
                var randomCaseLines = Doctran.Utilitys.StringUtils.ConvertToFileLineList(linesString.ToRandomCase());
                var fromRandomActual = block.ReturnObject(new IContained[] { }, randomCaseLines)
                    .ToList<IFortranObject>();
                makeAssertions(fromRandomActual);
            }
        }

        /// <summary>
        ///     Tests if <see cref="IFortranBlock.ReturnObject(IEnumerable{IContained}, List{Doctran.Helper.FileLine})" /> works
        ///     correctly. Requires sub-object to be passed to test how they are handled. Makes sure case invariance is adhered to.
        /// </summary>
        /// <param name="block">The block parser to be tested.</param>
        /// <param name="linesString">A string containing the source.</param>
        /// <param name="makeAssertions">An action that should include the assertions.</param>
        /// <param name="subObjects">
        ///     The sub-objects that should be pass to the
        ///     <see cref="IFortranBlock.ReturnObject(IEnumerable{IContained}, List{Doctran.Helper.FileLine})" /> routine.
        /// </param>
        public static void ReturnObjectWithSubObjectsCheck(IFortranBlock block, string linesString, Action<IEnumerable<IFortranObject>> makeAssertions, IEnumerable<IContained> subObjects)
        {
            var subObjectsList = subObjects as IList<IContained> ?? subObjects.ToList();

            {
                var lines = Doctran.Utilitys.StringUtils.ConvertToFileLineList(linesString);
                var actual = block.ReturnObject(subObjectsList, lines)
                    .ToList<IFortranObject>();
                makeAssertions(actual);
            }

            {
                var randomCaseLines = Doctran.Utilitys.StringUtils.ConvertToFileLineList(linesString.ToRandomCase());
                var fromRandomActual = block.ReturnObject(subObjectsList, randomCaseLines)
                    .ToList<IFortranObject>();
                makeAssertions(fromRandomActual);
            }
        }

        /// <summary>
        ///     Test to see if
        ///     <see cref="IFortranBlock.BlockStart(IEnumerable{IFortranBlock}, List{Doctran.Helper.FileLine}, int)" /> rejects the
        ///     correct sources.
        /// </summary>
        /// <param name="block">The block parser to be tested.</param>
        /// <param name="ancestors">The expected <see cref="IFortranBlock" /> ancestors of the block in question.</param>
        /// <param name="linesString">A string containing the source.</param>
        public static void StartRejectionCheck(IFortranBlock block, IEnumerable<IFortranBlock> ancestors, string linesString)
        {
            var lines = Doctran.Utilitys.StringUtils.ConvertToFileLineList(linesString);
            var ancestorsList = ancestors as IList<IFortranBlock> ?? ancestors.ToList();

            Assert.IsFalse(block.BlockStart(ancestorsList, lines, 0));

            var randomCaseLines = Doctran.Utilitys.StringUtils.ConvertToFileLineList(linesString.ToRandomCase());
            Assert.IsFalse(block.BlockStart(ancestorsList, randomCaseLines, 0));
        }
    }
}