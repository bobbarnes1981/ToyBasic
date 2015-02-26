namespace Basic.Errors
{
    /// <summary>
    /// Represents a expression error
    /// </summary>
    public class ExpressionError : Error
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ExpressionError"/> class.
        /// </summary>
        public ExpressionError(string message)
            : base(message)
        {
        }
    }
}
