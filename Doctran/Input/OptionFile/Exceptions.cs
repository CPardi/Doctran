namespace Doctran.Input.OptionFile
{
    using System;
    using Helper;

    public class WrongDepthException : Exception
    {
        public WrongDepthException(FileLine line, int expectedDepth, int actualDepth)
            : base("There is an error on line " + line.Number + ". The line '" + line.Text + "' is at level " + actualDepth + ", but was expected to be at level " + expectedDepth + ".")
        { }
    }
}
