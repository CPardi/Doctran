using Doctran.Helper;
using System.Collections.Generic;

namespace Doctran.Parsing
{
    public interface IFortranObject
    {
        List<FileLine> Lines { get; }
        IFortranObject Parent { get; set; }
        List<IFortranObject> SubObjects { get; }

        void AddSubObject(IFortranObject obj);

        void AddSubObjects(IEnumerable<IFortranObject> objs);

        T GoUpTillType<T>() where T : IFortranObject;

        List<IFortranObject> SubObjectsNotOfType<T>() where T : IFortranObject;

        List<T> SubObjectsOfType<T>() where T : IFortranObject;
    }
}