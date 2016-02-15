namespace Doctran.Test.XmlSerialization
{
    using Doctran.Parsing;

    internal interface ITestClass : IContained
    {
        bool ShouldCreate { get; }
    }
}