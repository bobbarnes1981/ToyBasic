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
            IFrame frame = new Frame();
            frame.Set("gosub_return", interpreter.Buffer.Current.Number);
            interpreter.Stack.Push(frame);
            interpreter.Buffer.Jump((int)m_lineNumber.Value(interpreter));
        }

        /// <summary>
        /// Gets or sets the line number to jump to
        /// </summary>
        public Number LineNumber
        {
            get { return m_lineNumber; }
            set { m_lineNumber = value; }
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
