﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Errors;

namespace Basic.Expressions
{
    public class StringConstant : Expression
    {
        public StringConstant(Operator op, string text)
            : base(op, text)
        {
        }

        public override object Add(IInterpreter interpreter, object value)
        {
            switch (value.GetType().Name)
            {
                case "String":
                    return ((string)value) + (string)m_obj;
                default:
                    throw new ExpressionError(string.Format("Cannot add '{0}'and '{1}'", m_obj.GetType().Name, value.GetType().Name));
            }
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
