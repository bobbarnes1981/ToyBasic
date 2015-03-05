namespace Basic.Commands.System
{
    /// <summary>
    /// Represents the system command 'Debug'
    /// </summary>
    public class Debug : Command
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Debug"/> class.
        /// </summary>
        public Debug()
            : base(Keywords.Debug, true)
        {
        }

        /// <summary>
        /// Executes the 'Run'command by calling execute on the interpreter interface
        /// </summary>
        /// <param name="interpreter"></param>
        public override void execute(IInterpreter interpreter)
        {
            interpreter.Execute(true);
        }

        /// <summary>
        /// Gets the text representation of the command
        /// </summary>
        public override string Text
        {
            get { return Keywords.Debug.ToString(); }
        }
    }
}
