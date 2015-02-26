namespace Basic.Errors
{
    /// <summary>
    /// Represents a heap error
    /// </summary>
    public class HeapError : Error
    {
        /// <summary>
        /// Creates a new instance of the <see cref="HeapError"/> class.
        /// </summary>
        public HeapError(string message)
            : base(message)
        {
        }
    }
}
