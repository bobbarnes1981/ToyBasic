using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.Commands
{
    public class Rem : Command
    {
        private string m_text;

        public Rem(string text)
            : base(Operation.Rem)
        {
            m_text = text;
        }

        public override void Execute(IInterpreter interpreter)
        {
        }

        public override string Text
        {
            get { return "Rem "+m_text; }
        }
    }
}
