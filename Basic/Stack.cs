using System.Collections.Generic;
using Basic.Factories;

namespace Basic
{
    public class Stack : IStack
    {
        private readonly IFactory<IFrame> m_frameFactory;

        private readonly Stack<IFrame> m_stack;

        public Stack(IFactory<IFrame> frameFactory)
        {
            m_frameFactory = frameFactory;
            m_stack = new Stack<IFrame>();
        }

        public IFrame Push()
        {
            IFrame frame = m_frameFactory.Build();
            m_stack.Push(frame);
            return frame;
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
