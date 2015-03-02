using Basic.Expressions;
using Basic.Factories;
using Basic.Types;

namespace Basic.Commands.Program
{
    /// <summary>
    /// Represents the program command 'For'
    /// </summary>
    public class For : Command
    {
        /// <summary>
        /// The variable to modify
        /// </summary>
        private readonly Variable m_variable;

        /// <summary>
        /// The start value
        /// </summary>
        private readonly INode m_start;

        /// <summary>
        /// The end value
        /// </summary>
        private readonly INode m_end;

        /// <summary>
        /// The step value
        /// </summary>
        private readonly INode m_step;

        /// <summary>
        /// Creates an instance of the <see cref="For"/> class.
        /// </summary>
        /// <param name="variable">the variable to modify</param>
        /// <param name="start">the start value</param>
        /// <param name="end">the end value</param>
        /// <param name="step">the step value</param>
        public For(Variable variable, INode start, INode end, INode step)
            : base(Keywords.For, false)
        {
            m_variable = variable;
            m_start = start;
            m_end = end;
            m_step = step;
        }

        /// <summary>
        /// Executes the 'For' command by initialising the variable on the heap and adding a frame to the stack provided by the interpreter heap and stack interfaces
        /// </summary>
        /// <param name="interpreter"></param>
        public override void execute(IInterpreter interpreter)
        {
            IFrame frame = interpreter.Stack.Push();
            frame.Set("for_end", m_end.Result(interpreter));
            frame.Set("for_step", m_step.Result(interpreter));
            frame.Set("for_line", interpreter.Buffer.Current.Number);
            frame.Set("for_var", m_variable.Name);
            m_variable.Set(interpreter, m_start.Result(interpreter));
        }

        /// <summary>
        /// Gets the text representation of the command
        /// </summary>
        public override string Text
        {
            get { return string.Format("{0} {1} = {2} {3} {4} {5} {6}", Keywords.For, m_variable.Text, m_start.Text, Keywords.To, m_end.Text, Keywords.Step, m_step.Text); }
        }
    }
}
