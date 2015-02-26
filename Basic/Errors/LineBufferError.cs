namespace Basic.Errors
{
    /// <summary>
    /// Represents a line buffer error
    /// </summary>
    public class LineBufferError : Error
    {
        /// <summary>
        /// Creates a new instance of the <see cref="LineBufferError"/> class.
        /// </summary>
        public LineBufferError(string message)
            : base(message)
        {
        }
    }
}
