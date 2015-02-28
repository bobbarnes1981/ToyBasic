using System.Collections.Generic;
using Basic.Errors;

namespace Basic
{
    public class Heap : IHeap
    {
        private readonly Dictionary<string, object> m_heap;

        public Heap()
        {
            m_heap = new Dictionary<string, object>();
        }

        public bool Exists(string name)
        {
            return m_heap.ContainsKey(name);
        }

        public void Set(string variable, object value)
        {
            if (variable == "Random")
            {
                throw new HeapError("Cannot set value of magic Random variable");
            }

            if (!m_heap.ContainsKey(variable))
            {
                m_heap.Add(variable, null);
            }

            m_heap[variable] = value;
        }

        public T Get<T>(string variable)
        {
            if (Get(variable).GetType() != typeof(T))
            {
                throw new HeapError(string.Format("Heap variable '{0}' is of type '{1}' not '{2}'", variable, Get(variable).GetType(), typeof(T)));
            }

            return (T)Get(variable);
        }

        public object Get(string variable)
        {
            if (variable == "Random")
            {
                return new System.Random().Next();
            }

            if (!m_heap.ContainsKey(variable))
            {
                throw new HeapError(string.Format("Heap variable '{0}' does not exist", variable));
            }

            return m_heap[variable];
        }

        public void Clear()
        {
            m_heap.Clear();
        }
    }
}
