using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.Commands
{
    public class If : Command
    {
        private IExpression m_expression;

        private ICommand m_command;

        public If(IExpression expression, ICommand command)
            : base(Operation.If)
        {
            m_expression = expression;
            m_command = command;
        }

        public override void Execute(IInterpreter interpreter)
        {
            object result = m_expression.Result(interpreter);
            bool output = false;
            switch (result.GetType().Name)
            {
                case "Int32":
                    output = (int)result != 0;
                    break;
                case "Boolean":
                    output = (bool)result;
                    break;
                default:
                    throw new NotImplementedException(result.GetType().Name);
            }
            if (output)
            {
                m_command.Execute(interpreter);
            }
        }
    }
}
