namespace Basic.Commands.Program
{
    /// <summary>
    /// Represents the program command 'Input'
    /// </summary>
    public class Input : Command
    {
        /// <summary>
        /// The name of the variable to store the input in
        /// </summary>
        private readonly string m_variable;

        /// <summary>
        /// Creates a new instance of the <see cref="Input"/> class.
        /// </summary>
        /// <param name="variable">The variable to store the input in</param>
        public Input(string variable)
            : base(Keywords.Input, false)
        {
            m_variable = variable;
        }

        /// <summary>
        /// Executes the 'Input' command by calling input on the interpreter console interface and storing the value in the interpreter heap interface
        /// </summary>
        /// <param name="interpreter">interpreter to provide the console and heap interfaces</param>
        public override void Execute(IInterpreter interpreter)
        {
            interpreter.Heap.Set(m_variable, interpreter.Console.Input());
        }

        /// <summary>
        /// Gets the text representation of the command
        /// </summary>
        public override string Text
        {
            get { return string.Format("{0} {1}", Keywords.Input, m_variable); }
        }
    }
}
