using System;

namespace Basic.Commands
{
    public class Save : Command
    {
        private readonly string m_filename;

        public Save(string filename)
            : base(Keyword.Save)
        {
            m_filename = filename;
        }

        public override void Execute(IInterpreter interpreter)
        {
            // generate file from buffer
            //interpreter.Disk.Save(m_filename);
            throw new NotImplementedException();
        }

        public override string Text
        {
            get { return string.Format("{0} {1}", Keyword.Save, m_filename); }
        }
    }
}
