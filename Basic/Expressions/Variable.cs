using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Errors;

namespace Basic.Expressions
{
    public class Variable : Expression
    {
        public Variable(Operator op, string variable)
            : base(op, variable)
        {
        }

        public override object Result(IInterpreter interpreter)
        {
            if (m_child != null)
            {
                return m_child.Result(interpreter, interpreter.Heap.Get((string)m_obj));
            }
            return interpreter.Heap.Get((string)m_obj);
        }

        public override object Add(IInterpreter interpreter, object value)
        {
            object other = interpreter.Heap.Get((string)m_obj);
            if (value.GetType().Name == other.GetType().Name)
            {
                switch (value.GetType().Name)
                {
                    case "Int32":
                        return ((int)value) + (int)other;
                    case "String":
                        return (string)value + (string)other;
                    default:
                        throw new ExpressionError(string.Format("Cannot add '{0}' and '{1}'", other.GetType().Name, value.GetType().Name));
                }
            }
            if (value.GetType().Name == "String")
            {
                return (string)value + other.ToString();
            }
            if (other.GetType().Name == "String")
            {
                return value.ToString() + (string)other;
            }
            throw new ExpressionError(string.Format("Cannot add '{0}' and '{1}'", other.GetType().Name, value.GetType().Name));
        }

        public override object Subtract(IInterpreter interpreter, object value)
        {
            throw new NotImplementedException();
        }

        public override object Multiply(IInterpreter interpreter, object value)
        {
            throw new NotImplementedException();
        }

        public override object Divide(IInterpreter interpreter, object value)
        {
            throw new NotImplementedException();
        }

        public override object Equals(IInterpreter interpreter, object value)
        {
            throw new NotImplementedException();
        }
    }
}
