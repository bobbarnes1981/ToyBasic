namespace Basic.Commands.System
{
    /// <summary>
    /// Represents the system command 'List'
    /// </summary>
    public class List : Command
    {
        /// <summary>
        /// Creates a new instance of the <see cref="List"/> class.
        /// </summary>
        public List()
            : base(Keywords.List, true)
        {
        }

        /// <summary>
        /// Executes the list command on the provided interpreter by stepping through the line buffer and displaying the line on the console output
        /// </summary>
        /// <param name="interpreter"></param>
        public override void execute(IInterpreter interpreter)
        {
            interpreter.Buffer.Reset();
            while (!interpreter.Buffer.End)
            {
                interpreter.Buffer.Next();
                interpreter.Console.Output(string.Format("{0}\r\n", interpreter.Buffer.Current));
            }
        }

        /// <summary>
        /// Gets the text representation of the command
        /// </summary>
        public override string Text
        {
            get { return Keywords.List.ToString(); }
        }
    }
}
