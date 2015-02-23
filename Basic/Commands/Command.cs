namespace Basic.Commands
{
    public abstract class Command : ICommand
    {
        private readonly Keyword m_keyword;

        private readonly bool m_isSystem;

        protected Command(Keyword keyword, bool isSystem)
        {
            m_keyword = keyword;
            m_isSystem = isSystem;
        }

        public Keyword Keyword
        {
            get { return m_keyword; }
        }

        public bool IsSystem
        {
            get { return m_isSystem; }
        }

        public abstract void Execute(IInterpreter interpreter);

        public abstract string Text { get; }
    }
}
