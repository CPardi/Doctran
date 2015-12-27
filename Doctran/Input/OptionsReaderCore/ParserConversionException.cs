namespace Doctran.Input.OptionsReaderCore
{
    using Parsing;

    internal class ParserConversionException : ParserException
    {
        public ParserConversionException(int startLine, int endLine, string parserName, string valueName, string message)
            : base(startLine, endLine, message)
        {
            ValueName = valueName;
        }

        public string ValueName { get; set; }
    }
}