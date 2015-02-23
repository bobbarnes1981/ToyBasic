namespace Basic.Commands
{
    public class Rem : Command
    {
        private readonly string m_text;

        public Rem(string text)
            : base(Keyword.Rem)
        {
            m_text = text;
        }

        public override void Execute(IInterpreter interpreter)
        {
        }

        public override string Text
        {
            get { return string.Format("{0}{1}", Keyword.Rem, m_text); }
        }
    }
}
