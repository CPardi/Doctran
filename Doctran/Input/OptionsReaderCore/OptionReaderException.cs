namespace Doctran.Input.OptionsReaderCore
{
    using System;

    public class OptionReaderException : ApplicationException
    {
        public OptionReaderException(int startLine, int endLine, string message)
            :base(message)
        {
            StartLine = startLine;
            EndLine = endLine;
        }

        public int StartLine { get; set; }
        public int EndLine { get; set; }
    }
}