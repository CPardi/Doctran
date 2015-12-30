namespace Doctran.Input.Options
{
    using System;

    public class OptionReaderException : ApplicationException
    {
        public OptionReaderException(int startLine, int endLine, string message)
            :base(message)
        {
            this.StartLine = startLine;
            this.EndLine = endLine;
        }

        public int StartLine { get; set; }
        public int EndLine { get; set; }
    }
}