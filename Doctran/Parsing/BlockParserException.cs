using System;

namespace Doctran.Parsing
{
    [Serializable]
    public class BlockParserException : ApplicationException
    {
        public BlockParserException(string message)
            : base(message)
        {
        }
    }
}
