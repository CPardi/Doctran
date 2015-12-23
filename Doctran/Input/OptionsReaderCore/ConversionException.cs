namespace Doctran.Input.OptionsReaderCore
{
    using System;

    internal class ConversionException : ApplicationException
    {
        public ConversionException(int startLine, int endLine, string valueName, string message)
        {
            StartLine = startLine;
            EndLine = endLine;
            ValueName = valueName;
            Message = message;
        }

        public int StartLine { get; set; }
        public int EndLine { get; set; }
        public string ValueName { get; set; }
        public string Message { get; set; }
    }
}