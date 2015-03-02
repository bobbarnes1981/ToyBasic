using Basic.Expressions;
using Basic.Types;

namespace Basic.Commands.Program
{
    /// <summary>
    /// Represents the program command 'Gosub'
    /// </summary>
    public class Gosub : Command
    {
        /// <summary>
        /// The line number to go to
        /// </summary>
        private Number m_lineNumber;

        /// <summary>
        /// Creates a new instance of the <see cref="Gosub"/> class.
        /// </summary>
        /// <param name="lineNumber">The line number to go to</param>
        public Gosub(Number lineNumber)
            : base(Keywords.Gosub, false)
        {
            m_lineNumber = lineNumber;
        }

        /// <summary>
        /// Executes the 'Gosub' command by adding a frame to the stack provided by the interpreter heap and stack interfaces
        /// </summary>
        /// <param name="interpreter"></param>
        public override void execute(IInterpreter interpreter)
        {
            if (interpreter.Stack.Count == 0
                || !interpreter.Stack.Peek().Exists("gosub_return")
                || interpreter.Stack.Peek().Get<int>("gosub_return") != interpreter.Buffer.Current.Number)
            {
                IFrame frame = new Frame();
                frame.Set("gosub_return", interpreter.Buffer.Current.Number);
                interpreter.Stack.Push(frame);
                interpreter.Buffer.Jump((int)m_lineNumber.Value(interpreter));
            }
            else
            {
                interpreter.Stack.Pop();
            }
        }

        /// <summary>
        /// Gets the text representation of the command
        /// </summary>
        public override string Text
        {
            get { return string.Format("{0} {1}", Keywords.Gosub, m_lineNumber.Text); }
        }
    }
}
