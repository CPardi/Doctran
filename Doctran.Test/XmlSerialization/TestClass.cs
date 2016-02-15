namespace Doctran.Test.XmlSerialization
{
    using Doctran.Parsing;

    internal class TestClass : ITestClass
    {
        public TestClass(bool shouldCreate = true)
        {
            this.ShouldCreate = shouldCreate;
        }

        public string ObjectName => "Test Class";

        public IContainer Parent { get; set; }

        public bool ShouldCreate { get; }
    }
}