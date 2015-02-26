namespace Basic.Commands.System
{
    /// <summary>
    /// Represents the system command 'Exit'
    /// </summary>
    public class Exit : Command
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Exit"/> class.
        /// </summary>
        public Exit()
            : base(Keywords.Exit, true)
        {
        }

        /// <summary>
        /// Executes the 'Exit'command by calling exit on the interpreter interface
        /// </summary>
        /// <param name="interpreter">interpreter to provide the exit command</param>
        public override void execute(IInterpreter interpreter)
        {
            interpreter.Exit();
        }

        /// <summary>
        /// Gets the text representation of the command
        /// </summary>
        public override string Text
        {
            get { return Keywords.Exit.ToString(); }
        }
    }
}
