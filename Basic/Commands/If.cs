using System;
using Basic.Expressions;

namespace Basic.Commands
{
    public class If : Command
    {
        private readonly IExpression m_expression;

        private readonly ICommand m_command;

        public If(IExpression expression, ICommand command)
            : base(Keyword.If)
        {
            m_expression = expression;
            m_command = command;
        }

        public override void Execute(IInterpreter interpreter)
        {
            object result = m_expression.Result();
            bool output;
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

        public override string Text
        {
            get { return string.Format("If {0} {1}", m_expression.Text, m_command.Text); }
        }
    }
}
