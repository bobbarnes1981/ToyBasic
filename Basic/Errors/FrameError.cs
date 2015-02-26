namespace Basic.Errors
{
    /// <summary>
    /// Represents a frame error
    /// </summary>
    public class FrameError : Error
    {
        /// <summary>
        /// Creates a new instance of the <see cref="FrameError"/> class.
        /// </summary>
        public FrameError(string message)
            : base(message)
        {
        }
    }
}
