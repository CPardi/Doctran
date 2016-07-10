namespace Doctran
{
    using System.Xml.Linq;

    public partial class Program
    {
        private static void SaveTiming(string name, long milliseonds)
        {
            TimingXml.Add(new XElement(name, milliseonds));
        }
    }
}