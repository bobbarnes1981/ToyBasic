using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic
{
    public interface IExpression
    {
        object Result(IInterpreter interpreter);
        object Result(IInterpreter interpreter, object value);
        void Child(IExpression expression);
        string Text { get; }
        object Add(IInterpreter interpreter, object value);
        object Subtract(IInterpreter interpreter, object value);
        object Multiply(IInterpreter interpreter, object value);
        object Divide(IInterpreter interpreter, object value);
    }
}
