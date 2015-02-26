using System.Collections.Generic;
using Basic.Errors;

namespace Basic
{
    /// <summary>
    /// Represents a stack frame
    /// </summary>
    public class Frame : IFrame
    {
        /// <summary>
        /// The stack frame
        /// </summary>
        private readonly Dictionary<string, object> m_frame;

        /// <summary>
        /// Create a new instance of the <see cref="Frame"/> class.
        /// </summary>
        public Frame()
        {
            m_frame = new Dictionary<string, object>();
        }

        /// <summary>
        /// Returns true if the specifie value exists
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Exists(string name)
        {
            return m_frame.ContainsKey(name);
        }

        /// <summary>
        /// Geta a value from the frame
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T Get<T>(string name)
        {
            if (Get(name).GetType() != typeof (T))
            {
                throw new FrameError(string.Format("Frame variable '{0}' is of type '{1}' not '{2}'", name, Get(name).GetType(), typeof(T)));
            }

            return (T)Get(name);
        }

        public object Get(string name)
        {
            if (!m_frame.ContainsKey(name))
            {
                throw new FrameError(string.Format("Frame variable '{0}' does not exist", name));
            }

            return m_frame[name];
        }

        /// <summary>
        /// Set a value in the frame
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Set(string name, object value)
        {
            if (!m_frame.ContainsKey(name))
            {
                m_frame.Add(name, null);
            }

            m_frame[name] = value;
        }
    }
}
