namespace Basic.Commands.System
{
    public class Load : Command
    {
        private readonly string m_filename;

        public Load(string filename)
            : base(Keywords.Load, true)
        {
            m_filename = filename;
        }

        public override void Execute(IInterpreter interpreter)
        {
            interpreter.Buffer.Clear();
            string[] lines = interpreter.Storage.Load(m_filename);
            foreach (string line in lines)
            {
                interpreter.ProcessInput(line);
            }
        }

        public override string Text
        {
            get { return string.Format("{0} {1}", Keywords.Load, m_filename); }
        }
    }
}
