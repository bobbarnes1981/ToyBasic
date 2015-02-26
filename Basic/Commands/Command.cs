using Basic.Errors;

namespace Basic.Commands
{
    /// <summary>
    /// Represents an abstract command
    /// </summary>
    public abstract class Command : ICommand
    {
        private readonly Keywords m_keyword;

        private readonly bool m_isSystem;

        /// <summary>
        /// Creates a new instance of the <see cref="Command"/> class.
        /// </summary>
        protected Command(Keywords keyword, bool isSystem)
        {
            m_keyword = keyword;
            m_isSystem = isSystem;
        }

        /// <summary>
        /// Gets the keyword for the command
        /// </summary>
        public Keywords Keyword
        {
            get { return m_keyword; }
        }

        /// <summary>
        /// Gets a value representing whether the command is a system command
        /// </summary>
        public bool IsSystem
        {
            get { return m_isSystem; }
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="interpreter"></param>
        public void Execute(IInterpreter interpreter)
        {
            try
            {
                execute(interpreter);
            }
            catch(Error error)
            {
                throw new CommandError(string.Format("Error executing '{0}'", Text), error);
            }
        }

        public abstract void execute(IInterpreter interpreter);

        /// <summary>
        /// Gets the text representation of the command
        /// </summary>
        public abstract string Text { get; }
    }
}
