using Basic.Expressions;

namespace Basic.Commands
{
    public class Print : Command
    {
        private readonly IExpression m_expression;

        public Print(IExpression expression)
            : base(Keyword.Print)
        {
            m_expression = expression;
        }

        public override void Execute(IInterpreter interpreter)
        {
            interpreter.Console.Output(string.Format("{0}\r\n", m_expression.Result()));
        }

        public override string Text
        {
            get { return string.Format("{0} \"{1}\"", Keyword.Print, m_expression.Text); }
        }
    }
}
