using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Errors;

namespace Basic
{
    public class Heap : IHeap
    {
        private Dictionary<string, object> m_heap;

        public Heap()
        {
            m_heap = new Dictionary<string, object>();
        }

        public void Set(string variable, object value)
        {
            if (!m_heap.ContainsKey(variable))
            {
                m_heap.Add(variable, null);
            }
            m_heap[variable] = value;
        }

        public object Get(string variable)
        {
            if (!m_heap.ContainsKey(variable))
            {
                throw new HeapError(string.Format("variable '{0}' does not exist", variable));
            }
            return m_heap[variable];
        }

        public void Clear()
        {
            m_heap.Clear();
        }
    }
}
