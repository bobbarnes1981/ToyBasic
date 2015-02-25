using System.Collections.Generic;

namespace Basic.Commands.System
{
    public class Save : Command
    {
        private readonly string m_filename;

        public Save(string filename)
            : base(Keywords.Save, true)
        {
            m_filename = filename;
        }

        public override void Execute(IInterpreter interpreter)
        {
            List<string> lines = new List<string>();
            interpreter.Buffer.Reset();
            while(!interpreter.Buffer.End)
            {
                lines.Add(interpreter.Buffer.Fetch.ToString());
            }
            interpreter.Storage.Save(m_filename, lines);
        }

        public override string Text
        {
            get { return string.Format("{0} {1}", Keywords.Save, m_filename); }
        }
    }
}
