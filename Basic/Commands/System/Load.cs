namespace Basic.Commands.System
{
    /// <summary>
    /// Represents the program command 'Load'
    /// </summary>
    public class Load : Command
    {
        private readonly string m_filename;

        /// <summary>
        /// Creates a new instance of the <see cref="Load"/> class.
        /// </summary>
        public Load(string filename)
            : base(Keywords.Load, true)
        {
            m_filename = filename;
        }

        /// <summary>
        /// Executes the load command by clearing the buffer and loading the lines from the specified file
        /// </summary>
        /// <param name="interpreter"></param>
        public override void Execute(IInterpreter interpreter)
        {
            interpreter.Buffer.Clear();
            string[] lines = interpreter.Storage.Load(m_filename);
            foreach (string line in lines)
            {
                interpreter.ProcessInput(line);
            }
        }

        /// <summary>
        /// Gets the text representation of the command
        /// </summary>
        public override string Text
        {
            get { return string.Format("{0} {1}", Keywords.Load, m_filename); }
        }
    }
}
