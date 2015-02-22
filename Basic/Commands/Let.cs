using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.Commands
{
    public class Let : Command
    {
        private string m_variable;

        private IExpression m_expression;

        public Let(string variable, IExpression expression)
            : base(Operation.Let)
        {
            m_variable = variable;
            m_expression = expression;
        }

        public override void Execute(IInterpreter interpreter)
        {
            interpreter.Heap.Set(m_variable, m_expression.Result(interpreter));
        }
    }
}
