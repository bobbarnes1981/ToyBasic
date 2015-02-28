namespace Basic.Commands.System
{
    /// <summary>
    /// Represents the system command 'Renumber'
    /// </summary>
    public class Renumber : Command
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Renumber"/> class.
        /// </summary>
        public Renumber()
            : base(Keywords.Renumber, true)
        {
        }

        /// <summary>
        /// Executes the 'Renumber' command by executing renumber on the buffer interface
        /// </summary>
        /// <param name="interpreter"></param>
        public override void execute(IInterpreter interpreter)
        {
            interpreter.Buffer.Renumber(interpreter);
        }

        /// <summary>
        /// Gets the text representation of the command
        /// </summary>
        public override string Text
        {
            get { return Keywords.Renumber.ToString(); }
        }
    }
}
