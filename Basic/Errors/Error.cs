using System;

namespace Basic.Errors
{
    /// <summary>
    /// Represents a generic error
    /// </summary>
    public class Error : Exception
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Error"/> class.
        /// </summary>
        public Error(string message)
            : base(message)
        {
        }
    }
}
