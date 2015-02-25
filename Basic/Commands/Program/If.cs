using Basic.Expressions;

namespace Basic.Commands.Program
{
    /// <summary>
    /// Represents the program command 'If'
    /// </summary>
    public class If : Command
    {
        /// <summary>
        /// The expression to evaluate
        /// </summary>
        private readonly IExpression m_expression;

        /// <summary>
        /// The command to execute if the expression is true
        /// </summary>
        private readonly ICommand m_command;

        /// <summary>
        /// Creates a new instance of the <see cref="If"/> class.
        /// </summary>
        /// <param name="expression">The expression to evaluate</param>
        /// <param name="command">The command to execute if the expression is true</param>
        public If(IExpression expression, ICommand command)
            : base(Keywords.If, false)
        {
            m_expression = expression;
            m_command = command;
        }

        public ICommand Command
        {
            get { return m_command; }
        }

        /// <summary>
        /// Executes the 'If' command by evaluating the expression and executing the command if the expression is true
        /// </summary>
        /// <param name="interpreter">interpreter to execute the command on</param>
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
                case "String":
                    output = !string.IsNullOrEmpty((string)result);
                    break;
                default:
                    throw new Errors.Expression(string.Format("Unhandled type '{0}'", result.GetType().Name));
            }
            if (output)
            {
                m_command.Execute(interpreter);
            }
        }

        /// <summary>
        /// Gets the text representation of the command
        /// </summary>
        public override string Text
        {
            get { return string.Format("If {0} {1}", m_expression.Text, m_command.Text); }
        }
    }
}
