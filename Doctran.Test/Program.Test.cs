namespace Doctran.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class ProgramTest
    {
        private string _currentDir;
        private readonly StringWriter _sw = new StringWriter();
        private readonly TextReader _tr = new StringReader("");

        [SetUp]
        public void Setup()
        {
            _currentDir = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(@"C:\Documents\Programming\VirtualBox_TestBed\TestFiles");
        }

        [TearDown]
        public void TearDown()
        {
            Directory.SetCurrentDirectory(_currentDir);
        }

        [Test]
        [Ignore("Takes too long to run.")]
        [Category("Full Program")]
        [TestCase(@"mine\Vectors\")]
        [TestCase(@"mine\Timers\")]
        [TestCase(@"mine\Functions\")]
        [TestCase(@"mine\f2003\")]
        public void BasicTest(string testPath)
        {
            Console.WriteLine(TestContext.CurrentContext.TestDirectory);

            Console.SetOut(_sw);
            Console.SetIn(_tr);

            var soureDir = testPath;
            var args = Directory.GetFiles(soureDir + @"", "*.f90", SearchOption.AllDirectories).ToList();

            args.Add("-o");
            args.Add($@"{testPath}\Docs");

            args.Add("--overwrite");
            args.Add("--project_info");
            args.Add(soureDir + @"Doctran\project.info");
            Program.Main(args.ToArray());
        }
    }
}
