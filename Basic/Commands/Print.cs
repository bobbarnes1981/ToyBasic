using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.Commands
{
    public class Print : ICommand
    {
        private IExpression m_expression;

        public Print(IExpression expression)
        {
            m_expression = expression;
        }

        public void Execute(IInterpreter interpreter)
        {
            interpreter.Display.Output(m_expression.Result(interpreter).ToString());
        }
    }
}
