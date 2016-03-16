namespace Doctran.Test.Utilitys.ParsingUtils
{
    using Doctran.Utilitys;
    using NUnit.Framework;

    [TestFixture]
    public class ReplaceMacrosTest
    {
        [Test]
        [TestCase(
            "|name|",
            "<macro name=\"name\" />",
            TestName = "Optionless name macro.")]
        [TestCase(
            "|blockname|",
            "<macro name=\"blockname\" />",
            TestName = "Optionless block-name macro.")]
        [TestCase(
            "|list,program|",
            "<macro name=\"list\"><option>program</option></macro>",
            TestName = "Only macro and one property. No spacing.")]
        [TestCase(
            "|list,program,recursive|",
            "<macro name=\"list\"><option>program</option><option>recursive</option></macro>",
            TestName = "Only macro and two properties. No spacing.")]
        [TestCase(
            "| list , program |",
            "<macro name=\"list\"><option>program</option></macro>",
            TestName = "Only macro and one property. With spacing.")]
        [TestCase(
            "| list , program , recursive |",
            "<macro name=\"list\"><option>program</option><option>recursive</option></macro>",
            TestName = "Only macro and two properties. With spacing.")]
        [TestCase(
            " * program list\n   * |list,program|\n * module list\n   * |list,module|",
            " * program list\n   * <macro name=\"list\"><option>program</option></macro>\n * module list\n   * <macro name=\"list\"><option>module</option></macro>",
            TestName = "Macro within text.")]
        public void ModifiedOutput(string source, string expected)
        {
            // Run the replacement on the string case and compare results.
            var actual = ParsingUtils.ReplaceMacros(source);
            Assert.AreEqual(expected, actual, "Lower-case case failed.");

            // Run the replacement on the string with random case and compare the lowercase results.
            var actualRandomCase = ParsingUtils.ReplaceMacros(source.ToRandomCase()).ToLower();
            Assert.AreEqual(expected, actualRandomCase, "Randomized case failed.");
        }

        [Test]
        [TestCase("", TestName = "Empty in, empty out.")]
        [TestCase("|zzz, program|", TestName = "Not a macro name.")]
        public void UnchangedOutput(string source)
        {
            var actual = ParsingUtils.ReplaceMacros(source);
            Assert.AreEqual(source, actual);
        }
    }
}