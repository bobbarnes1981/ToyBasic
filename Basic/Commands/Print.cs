using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.Commands
{
    public class Print : Command
    {
        private IExpression m_expression;

        public Print(IExpression expression)
            : base(Operation.Print)
        {
            m_expression = expression;
        }

        public override void Execute(IInterpreter interpreter)
        {
            interpreter.Display.Output(m_expression.Result(interpreter).ToString());
        }

        public override string Text
        {
            get { return "Print \""+m_expression.Text+"\""; }
        }
    }
}
