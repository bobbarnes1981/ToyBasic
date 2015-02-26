namespace Basic.Commands.System
{
    /// <summary>
    /// Represents the program command 'New'
    /// </summary>
    public class New : Command
    {
        /// <summary>
        /// Creates a new instance of the <see cref="New"/> class.
        /// </summary>
        public New()
            : base(Keywords.New, true)
        {
        }

        /// <summary>
        /// Executes the 'New' command by clearing the heap and the buffer
        /// </summary>
        /// <param name="interpreter"></param>
        public override void execute(IInterpreter interpreter)
        {
            interpreter.Heap.Clear();
            interpreter.Buffer.Clear();
        }

        /// <summary>
        /// Gets the text representation of the command
        /// </summary>
        public override string Text
        {
            get { return Keywords.New.ToString(); }
        }
    }
}
