namespace Doctran.ParsingElements
{
    using System.Xml.Linq;
    using Parsing;

    public interface IDescription : IFortranObject
    {
        XElement Basic { get; }
        XElement Detailed { get; }
    }
}