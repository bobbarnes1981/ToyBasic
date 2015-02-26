using System;

namespace Basic.Errors
{
    /// <summary>
    /// Represents a generic error
    /// </summary>
    public class Error : Exception
    {
        private Error m_innerError;

        /// <summary>
        /// Creates a new instance of the <see cref="Error"/> class.
        /// </summary>
        public Error(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Error"/> class.
        /// </summary>
        public Error(string message, Error innerError)
            : base(message)
        {
            m_innerError = innerError;
        }

        public Error InnerError { get { return m_innerError; } }
    }
}
