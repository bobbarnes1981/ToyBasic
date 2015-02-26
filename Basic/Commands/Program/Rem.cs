namespace Basic.Commands.Program
{
    /// <summary>
    /// Represents the program command 'Rem'
    /// </summary>
    public class Rem : Command
    {
        /// <summary>
        /// The text to remember
        /// </summary>
        private readonly string m_text;

        /// <summary>
        /// Creates a new instance of the <see cref="Rem"/> class.
        /// </summary>
        /// <param name="text">The text to remember</param>
        public Rem(string text)
            : base(Keywords.Rem, false)
        {
            m_text = text;
        }

        /// <summary>
        /// Executes the 'Rem' command by doing nothing
        /// </summary>
        /// <param name="interpreter"></param>
        public override void execute(IInterpreter interpreter)
        {
        }

        /// <summary>
        /// Get the text representation of the command
        /// </summary>
        public override string Text
        {
            get { return string.Format("{0}{1}", Keywords.Rem, m_text); }
        }
    }
}
