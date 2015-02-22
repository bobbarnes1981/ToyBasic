using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Errors;

namespace Basic.Expressions
{
    public class Number : Expression
    {
        public Number(Operator op, int number)
            : base(op, number)
        {
        }

        public override string Text
        {
            get
            {
                string output = string.Empty;
                if (m_operator != Operator.None)
                {
                    // todo: convert to symbol
                    output += m_operator.ToString() + " ";
                }
                output += ((int)m_obj).ToString() + " ";
                if (m_child != null)
                {
                    output += m_child.Text;
                }
                return output;
            }
        }

        public override object Add(IInterpreter interpreter, object value)
        {
            switch(value.GetType().Name)
            {
                case "Int32":
                    return ((int)value) + (int)m_obj;
                default:
                    throw new ExpressionError(string.Format("Cannot add '{0}' and '{1}'", m_obj.GetType().Name, value.GetType().Name));
            }
        }

        public override object Subtract(IInterpreter interpreter, object value)
        {
            switch (value.GetType().Name)
            {
                case "Int32":
                    return ((int)value) - (int)m_obj;
                default:
                    throw new ExpressionError(string.Format("Cannot subtract '{0}' and '{1}'", m_obj.GetType().Name, value.GetType().Name));
            }
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
            switch (value.GetType().Name)
            {
                case "Int32":
                    return ((int)value) == (int)m_obj;
                default:
                    throw new ExpressionError(string.Format("Cannot compare '{0}' and '{1}'", m_obj.GetType().Name, value.GetType().Name));
            }
        }
    }
}
