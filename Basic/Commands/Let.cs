using Basic.Expressions;

namespace Basic.Commands
{
    public class Let : Command
    {
        private readonly string m_variable;

        private readonly IExpression m_expression;

        public Let(string variable, IExpression expression)
            : base(Keyword.Let)
        {
            m_variable = variable;
            m_expression = expression;
        }

        public override void Execute(IInterpreter interpreter)
        {
            interpreter.Heap.Set(m_variable, m_expression.Result());
        }

        public override string Text
        {
            get { return string.Format("{0} {1} = {2}", Keyword.Let, m_variable, m_expression.Text); }
        }
    }
}
