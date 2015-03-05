namespace Basic.Commands.Program
{
    /// <summary>
    /// Represents the program command 'Break'
    /// </summary>
    public class Break : Command
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Break"/> class.
        /// </summary>
        public Break()
            : base(Keywords.Break, false)
        {
        }

        /// <summary>
        /// Executes the 'Break' command by calling clear on the interpreter console interface
        /// </summary>
        /// <param name="interpreter">interpreter to provide the console interface</param>
        public override void execute(IInterpreter interpreter)
        {
            interpreter.Console.Clear();
        }

        /// <summary>
        /// Gets the text representation of the command
        /// </summary>
        public override string Text
        {
            get { return Keywords.Break.ToString(); }
        }
    }
}
