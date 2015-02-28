using Basic.Errors;
using Basic.Expressions;

namespace Basic.Types
{
    /// <summary>
    /// Represents a variable type
    /// </summary>
    public class Variable : Type
    {
        public static readonly char PREFIX = '$';

        private readonly INode m_index;

        public Variable(string name, INode index)
            : base(name)
        {
            m_index = index;
        }

        public INode Index
        {
            get { return m_index; }
        }

        public string Name { get { return (string)m_value; } }

        public override object Value(IInterpreter interpreter)
        {
            return Get(interpreter);
        }

        public object Get(IInterpreter interpreter)
        {
            if (m_index != null)
            {
                object value = interpreter.Heap.Get(Name);
                if (!value.GetType().IsArray)
                {
                    throw new TypeError(string.Format("variable '{0}' is not an array", Name));
                }

                object[] array = (object[])value;
                if (array.Length <= (int)m_index.Result(interpreter))
                {
                    throw new TypeError(string.Format("variable '{0}' has '{1}' elements, invalid index '{2}'", Name, m_index));
                }

                return ((object[])value)[(int)m_index.Result(interpreter)];
            }
            else
            {
                return interpreter.Heap.Get(Name);
            }
        }

        public void Set(IInterpreter interpreter, object input)
        {
            if (m_index != null)
            {
                object value = interpreter.Heap.Get(Name);
                if (!value.GetType().IsArray)
                {
                    throw new TypeError(string.Format("variable '{0}' is not an array", Name));
                }

                object[] array = (object[])value;
                if (array.Length <= (int)m_index.Result(interpreter))
                {
                    throw new TypeError(string.Format("variable '{0}' has '{1}' elements, invalid index '{2}'", Name, m_index));
                }

                array[(int)m_index.Result(interpreter)] = input;
            }
            else
            {
                interpreter.Heap.Set(Name, input);
            }
        }

        public override string Text
        {
            get
            {
                if (m_index != null)
                {
                    return string.Format("{0}{1}[{2}]", PREFIX, m_value, m_index.Text);
                }

                return string.Format("{0}{1}", PREFIX, m_value);
            }
        }
    }
}
