namespace Basic.Commands
{
    public class Load : Command
    {
        private readonly string m_filename;

        public Load(string filename)
            : base(Keyword.Load)
        {
            m_filename = filename;
        }

        public override void Execute(IInterpreter interpreter)
        {
            throw new System.NotImplementedException();
        }

        public override string Text
        {
            get { return string.Format("{0} {1}", Keyword.Load, m_filename); }
        }
    }
}
