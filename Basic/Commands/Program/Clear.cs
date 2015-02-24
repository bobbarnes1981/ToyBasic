namespace Basic.Commands.Program
{
    /// <summary>
    /// Represents the program command 'Clear'
    /// </summary>
    public class Clear : Command
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Clear"/> class.
        /// </summary>
        public Clear()
            : base(Keywords.Clear, false)
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
