namespace Basic.Commands
{
    public abstract class Command : ICommand
    {
        private readonly Keywords m_keyword;

        private readonly bool m_isSystem;

        protected Command(Keywords keyword, bool isSystem)
        {
            m_keyword = keyword;
            m_isSystem = isSystem;
        }

        public Keywords Keyword
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
