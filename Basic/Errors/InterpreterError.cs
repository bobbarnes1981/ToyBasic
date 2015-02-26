namespace Basic.Errors
{
    /// <summary>
    /// Represents an interpreter error
    /// </summary>
    public class InterpreterError : Error
    {
        /// <summary>
        /// Creates a new instance of the <see cref="InterpreterError"/> class.
        /// </summary>
        public InterpreterError(string message)
            : base(message)
        {
        }

        public InterpreterError(string message, Error innerError)
            : base(message, innerError)
        {
        }
    }
}
