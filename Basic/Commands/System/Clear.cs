namespace Basic.Commands.System
{
    /// <summary>
    /// Represents the system command 'Clear'
    /// </summary>
    public class Clear : Command
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Clear"/> class.
        /// </summary>
        public Clear()
            : base(Keywords.Clear, true)
        {
        }

        /// <summary>
        /// Executes the 'Clear' command by calling clear on the interpreter console interface
        /// </summary>
        /// <param name="interpreter">interpreter to provide the console interface</param>
        public override void Execute(IInterpreter interpreter)
        {
            interpreter.Console.Clear();
        }

        /// <summary>
        /// Gets the text representation of the command
        /// </summary>
        public override string Text
        {
            get { return Keywords.Clear.ToString(); }
        }
    }
}
