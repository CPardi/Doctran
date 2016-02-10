namespace Doctran.Helper
{
    using System;
    using System.Collections.Generic;

    public class TypeSwitch
    {
        private readonly Dictionary<Type, Action<object>> _matches = new Dictionary<Type, Action<object>>();

        public TypeSwitch Case<T>(Action<T> action)
        {
            _matches.Add(typeof(T), x => action((T)x));
            return this;
        }

        public void Switch(object x)
        {
            _matches[x.GetType()](x);
        }
    }
}