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
        private readonly INode m_expression;

        /// <summary>
        /// Creates a new instance of the <see cref="Print"/> class.
        /// </summary>
        /// <param name="expression">The expression to evaluate</param>
        public Print(INode expression)
            : base(Keywords.Print, false)
        {
            m_expression = expression;
        }

        /// <summary>
        /// Executes the 'Print' command by evaluating the epresssion and calling output on the interpreter console interface
        /// </summary>
        /// <param name="interpreter">interpreter to provide the console interface</param>
        public override void execute(IInterpreter interpreter)
        {
            interpreter.Console.Output(string.Format("{0}\r\n", m_expression.Result(interpreter)));
        }

        /// <summary>
        /// Gets the text representation of the command
        /// </summary>
        public override string Text
        {
            get { return string.Format("{0} {1}", Keywords.Print, m_expression.Text); }
        }
    }
}
