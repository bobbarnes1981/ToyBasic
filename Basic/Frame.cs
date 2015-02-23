using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic
{
    public class Frame : IFrame
    {
        private Dictionary<string, object> m_frame;

        public Frame()
        {
            m_frame = new Dictionary<string, object>();
        }

        public T Get<T>(string name)
        {
            return (T)m_frame[name];
        }

        public void Set<T>(string name, T value)
        {
            if (!m_frame.ContainsKey(name))
            {
                m_frame.Add(name, null);
            }
            m_frame[name] = value;
        }
    }
}
