using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doctran.ParsingElements
{
    using Parsing;

    public interface IScope
    {
        T GetObjectFromName<T>(string name)
            where T : IFortranObject;
    }

    public abstract class Scope : IScope
    {
        protected Dictionary<string, IFortranObject> ObjectStore { get; } = new Dictionary<string, IFortranObject>();

        public T GetObjectFromName<T>(string name)
            where T : IFortranObject
        {
            IFortranObject obj;
            if (!this.ObjectStore.TryGetValue(name, out obj))
            {
                throw new ArgumentException($"'{name}' could not be found in scope.");
            }

            if (!(obj is T))
            {
                throw new InvalidCastException($"'{name}' was found in scope, but was not of expected type '{Names.OfType<T>()}'.");
            }

            return (T)obj;
        }
    }
}
