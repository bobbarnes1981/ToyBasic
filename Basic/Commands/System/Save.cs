namespace Basic.Commands.System
{
    public class Save : Command
    {
        private readonly string m_filename;

        public Save(string filename)
            : base(Keyword.Save, true)
        {
            m_filename = filename;
        }

        public override void Execute(IInterpreter interpreter)
        {
            // generate file from buffer
            //interpreter.Disk.Save(m_filename);
            throw new global::System.NotImplementedException();
        }

        public override string Text
        {
            get { return string.Format("{0} {1}", Keyword.Save, m_filename); }
        }
    }
}
