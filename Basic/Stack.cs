using System.Collections.Generic;

namespace Basic
{
    public class Stack : IStack
    {
        private readonly Stack<IFrame> m_stack;

        public Stack()
        {
            m_stack = new Stack<IFrame>();
        }

        public void Push(IFrame frame)
        {
            m_stack.Push(frame);
        }

        public IFrame Pop()
        {
            return m_stack.Pop();
        }

        public IFrame Peek()
        {
            return m_stack.Peek();
        }

        public int Count
        {
            get
            {
                return m_stack.Count;
            }
        }
    }
}
