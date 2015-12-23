using System.Xml.Linq;

namespace Doctran.Parsing.BuiltIn.FortranObjects
{
    public interface IDescription
    {
        XElement Basic { get; }
        XElement Detailed { get; }
    }
}