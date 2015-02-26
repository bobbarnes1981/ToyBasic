namespace Basic.Errors
{
    /// <summary>
    /// Represents a command error
    /// </summary>
    public class CommandError : Error
    {
        /// <summary>
        /// Creates a new instance of the <see cref="CommandError"/> class.
        /// </summary>
        public CommandError(string message)
            : base(message)
        {
        }
    }
}
