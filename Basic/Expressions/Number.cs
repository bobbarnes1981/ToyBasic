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
        public Number(int number)
            : base(number)
        {
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
