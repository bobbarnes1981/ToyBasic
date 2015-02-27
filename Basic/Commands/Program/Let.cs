using Basic.Expressions;
using Basic.Types;

namespace Basic.Commands.Program
{
    /// <summary>
    /// Represents the program command 'Let'
    /// </summary>
    public class Let : Command
    {
        /// <summary>
        /// The variable to assign to
        /// </summary>
        private readonly Variable m_variable;

        /// <summary>
        /// The expression to assign to the variable
        /// </summary>
        private readonly INode m_expression;

        /// <summary>
        /// Creates a new instance of the <see cref="Let"/> class.
        /// </summary>
        /// <param name="variable">The variable to assign to</param>
        /// <param name="expression">The expression to assign to the variable</param>
        public Let(Variable variable, INode expression)
            : base(Keywords.Let, false)
        {
            m_variable = variable;
            m_expression = expression;
        }

        /// <summary>
        /// Executes the 'Let' command by evaluating the expression and assigning it using the interpreter heap interface
        /// </summary>
        /// <param name="interpreter">interpreter to provide the heap interface</param>
        public override void execute(IInterpreter interpreter)
        {
            m_variable.Set(m_expression.Result());
        }

        /// <summary>
        /// gets the text representation of the command
        /// </summary>
        public override string Text
        {
            get { return string.Format("{0} {1} = {2}", Keywords.Let, m_variable.Text, m_expression.Text); }
        }
    }
}
