namespace Basic.Errors
{
    /// <summary>
    /// Represents a line error
    /// </summary>
    public class LineError : Error
    {
        /// <summary>
        /// Creates a new instance of the <see cref="LineError"/> class.
        /// </summary>
        public LineError(string message)
            : base(message)
        {
        }
    }
}
