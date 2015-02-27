using Basic.Errors;
using Basic.Expressions;

namespace Basic.Types
{
    /// <summary>
    /// Represents a variable type
    /// </summary>
    public class Variable : Type
    {
        private readonly IInterpreter m_interpreter;

        public static readonly char PREFIX = '$';

        private readonly INode m_index;

        public Variable(IInterpreter interpreter, string variable, INode index)
            : base(variable)
        {
            m_interpreter = interpreter;
            m_index = index;
        }

        public INode Index
        {
            get { return m_index; }
        }

        public string Name { get { return (string)m_value; } }

        public override object Value()
        {
            if ((int)m_index.Result() >= 0)
            {
                object value = m_interpreter.Heap.Get(Name);
                if (!value.GetType().IsArray)
                {
                    throw new TypeError(string.Format("variable '{0}' is not an array", Name));
                }

                object[] array = (object[])value;
                if (array.Length <= (int)m_index.Result())
                {
                    throw new TypeError(string.Format("variable '{0}' has '{1}' elements, invalid index '{2}'", Name, m_index));
                }

                return ((object[])value)[(int)m_index.Result()];
            }
            else
            {
                return m_interpreter.Heap.Get(Name);
            }
        }

        public void Set(object input)
        {
            if ((int)m_index.Result() >= 0)
            {
                object value = m_interpreter.Heap.Get(Name);
                if (!value.GetType().IsArray)
                {
                    throw new TypeError(string.Format("variable '{0}' is not an array", Name));
                }

                object[] array = (object[])value;
                if (array.Length <= (int)m_index.Result())
                {
                    throw new TypeError(string.Format("variable '{0}' has '{1}' elements, invalid index '{2}'", Name, m_index));
                }

                array[(int)m_index.Result()] = input;
            }
            else
            {
                m_interpreter.Heap.Set(Name, input);
            }
        }

        public override string Text
        {
            get
            {
                if ((int)m_index.Result() >= 0)
                {
                    return string.Format("{0}{1}[{2}]", PREFIX, m_value, m_index.Text);
                }

                return string.Format("{0}{1}", PREFIX, m_value);
            }
        }
    }
}
