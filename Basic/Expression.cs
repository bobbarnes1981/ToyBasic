using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Errors;

namespace Basic
{
    public abstract class Expression : IExpression
    {
        protected object m_obj;
        protected Operator m_operator;
        protected IExpression m_child;

        protected Expression(object obj)
            : this(Operator.None, obj)
        {
        }

        protected Expression(Operator op, object obj)
        {
            m_operator = op;
            m_obj = obj;
        }

        public void Child(IExpression expression)
        {
            m_child = expression;
        }

        public virtual object Result(IInterpreter interpreter)
        {
            if (m_child != null)
            {
                return m_child.Result(interpreter, m_obj);
            }
            return m_obj;
        }

        public object Result(IInterpreter interpreter, object value)
        {
            object result;
            switch (m_operator)
            {
                case Operator.Add:
                    result = Add(interpreter, value);
                    break;
                case Operator.Subtract:
                    result = Subtract(interpreter, value);
                    break;
                case Operator.Equals:
                    result = Equals(interpreter, value);
                    break;
                default:
                    throw new ExpressionError(string.Format("Unhandled operator '{0}'", m_operator));
            }
            if (m_child != null)
            {
                result = m_child.Result(interpreter, result);
            }
            return result;
        }

        public abstract object Add(IInterpreter interpreter, object value);
        public abstract object Subtract(IInterpreter interpreter, object value);
        public abstract object Multiply(IInterpreter interpreter, object value);
        public abstract object Divide(IInterpreter interpreter, object value);
        public abstract object Equals(IInterpreter interpreter, object value);
    }
}
