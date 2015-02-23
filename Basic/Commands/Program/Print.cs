using Basic.Expressions;

namespace Basic.Commands.Program
{
    /// <summary>
    /// Represents the program command 'Print'
    /// </summary>
    public class Print : Command
    {
        /// <summary>
        /// The expression to evaluate
        /// </summary>
        private readonly IExpression m_expression;

        /// <summary>
        /// Creates a new instance of the <see cref="Print"/> class.
        /// </summary>
        /// <param name="expression">The expression to evaluate</param>
        public Print(IExpression expression)
            : base(Keyword.Print, false)
        {
            m_expression = expression;
        }

        /// <summary>
        /// Executes the 'Print' command by evaluating the epresssion and calling output on the interpreter console interface
        /// </summary>
        /// <param name="interpreter">interpreter to provide the console interface</param>
        public override void Execute(IInterpreter interpreter)
        {
            interpreter.Console.Output(string.Format("{0}\r\n", m_expression.Result()));
        }

        /// <summary>
        /// Gets the text representation of the command
        /// </summary>
        public override string Text
        {
            get { return string.Format("{0} \"{1}\"", Keyword.Print, m_expression.Text); }
        }
    }
}
