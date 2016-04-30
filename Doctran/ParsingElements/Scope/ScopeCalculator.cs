namespace Doctran.ParsingElements.Scope
{
    using System.Collections.Generic;
    using Parsing;

    public delegate IEnumerable<IdentifierObjectPair> ScopeCalculator(IFortranObject obj);
}