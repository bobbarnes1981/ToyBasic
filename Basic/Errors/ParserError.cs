namespace Basic.Errors
{
    /// <summary>
    /// Represents a parse error
    /// </summary>
    public class ParserError : Error
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ParserError"/> class.
        /// </summary>
        public ParserError(string message)
            : base(message)
        {
        }
    }
}
