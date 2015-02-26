namespace Basic.Errors
{
    /// <summary>
    /// Represents a stack error
    /// </summary>
    public class StackError : Error
    {
        /// <summary>
        /// Creates a new instance of the <see cref="StackError"/> class.
        /// </summary>
        public StackError(string message)
            : base(message)
        {
        }
    }
}
