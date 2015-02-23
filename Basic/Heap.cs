using System.Collections.Generic;

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
                throw new Errors.Heap(string.Format("variable '{0}' does not exist", variable));
            }
            return m_heap[variable];
        }

        public void Clear()
        {
            m_heap.Clear();
        }
    }
}
