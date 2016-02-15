namespace Doctran.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Utilitys;

    public class CompareRootTypes : EqualityComparer<Type>
    {
        private readonly IEnumerable<Type> _types;

        public CompareRootTypes(IEnumerable<Type> types)
        {
            _types = types;
        }

        public override bool Equals(Type x, Type y)
        {
            var xLowestType = x.GetTypeAndBaseTypes().First(t => _types.Contains(t));
            var yLowestType = x.GetTypeAndBaseTypes().First(t => _types.Contains(t));

            return xLowestType == yLowestType;
        }

        public override int GetHashCode(Type obj) => obj?.GetTypeAndBaseTypesAndInterfaces().FirstOrDefault(t => _types.Contains(t))?.GetHashCode() ?? obj?.GetHashCode() ?? 0;
    }
}