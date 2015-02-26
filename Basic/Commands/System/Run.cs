namespace Basic.Commands.System
{
    /// <summary>
    /// Represents the system command 'Run'
    /// </summary>
    public class Run : Command
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Run"/> class.
        /// </summary>
        public Run()
            : base(Keywords.Run, true)
        {
        }

        /// <summary>
        /// Executes the 'Run'command by calling execute on the interpreter interface
        /// </summary>
        /// <param name="interpreter"></param>
        public override void Execute(IInterpreter interpreter)
        {
            interpreter.Execute();
        }

        /// <summary>
        /// Gets the text representation of the command
        /// </summary>
        public override string Text
        {
            get { return Keywords.Run.ToString(); }
        }
    }
}
