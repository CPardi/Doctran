namespace Doctran.Parsing
{
    using System;

    public class ParserException : ApplicationException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ParserException" /> class.
        /// </summary>
        /// <param name="startLine">The number of the first line on which the incorrect text was found.</param>
        /// <param name="endLine">The number of the last line on which the incorrect text was found.</param>
        /// <param name="message">A description of the exception that occured.</param>
        public ParserException(int startLine, int endLine, string message)
            : base(message)
        {
            this.StartLine = startLine;
            this.EndLine = endLine;
        }

        /// <summary>
        ///     The number of the last line on which the incorrect text was found.
        /// </summary>
        public int EndLine { get; }

        /// <summary>
        ///     The number of the first line on which the incorrect text was found.
        /// </summary>
        public int StartLine { get; }
    }
}