namespace Basic.Commands
{
    public abstract class Command : ICommand
    {
        private readonly Keyword m_keyword;

        protected Command(Keyword keyword)
        {
            m_keyword = keyword;
        }

        public Keyword Keyword
        {
            get { return m_keyword; }
        }

        public abstract void Execute(IInterpreter interpreter);

        public abstract string Text { get; }
    }
}
