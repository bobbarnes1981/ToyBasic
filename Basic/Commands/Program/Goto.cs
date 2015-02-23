namespace Basic.Commands.Program
{
    /// <summary>
    /// Represents the program command 'Goto'
    /// </summary>
    public class Goto : Command
    {
        /// <summary>
        /// The line number to go to
        /// </summary>
        private int m_lineNumber;

        /// <summary>
        /// Creates a new instance of the <see cref="Goto"/> class.
        /// </summary>
        /// <param name="lineNumber">The line number to go to</param>
        public Goto(int lineNumber)
            : base(Keyword.Goto, false)
        {
            m_lineNumber = lineNumber;
        }

        /// <summary>
        /// Executes the 'Goto' command by calling jump on the interpreter buffer interface
        /// </summary>
        /// <param name="interpreter">interpreter to provide the buffer interface</param>
        public override void Execute(IInterpreter interpreter)
        {
            interpreter.Buffer.Jump(m_lineNumber);
        }

        /// <summary>
        /// Gets the text representation of the command
        /// </summary>
        public override string Text
        {
            get { return string.Format("{0} {1}", Keyword.Goto, m_lineNumber); }
        }
    }
}
