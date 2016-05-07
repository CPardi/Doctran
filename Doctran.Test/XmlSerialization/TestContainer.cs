namespace Doctran.Test.XmlSerialization
{
    using System.Collections.Generic;
    using Doctran.Parsing;
    using Doctran.ParsingElements;

    internal class TestContainer : Container, ITestClass
    {
        public TestContainer(bool shouldCreate, IEnumerable<IContained> subObjects)
            : base(subObjects)
        {
            this.ShouldCreate = shouldCreate;
        }

        public new string ObjectName => "Test Container";

        public bool ShouldCreate { get; }

        public IContainer Parent { get; set; }
    }
}