using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.Commands
{
    // not a real command
    public class Renumber : Command
    {
        public Renumber()
            : base(Keyword.Renumber)
        {
        }

        public override void Execute(IInterpreter interpreter)
        {
            interpreter.Buffer.Renumber(10);
        }

        public override string Text
        {
            get { return "Renumber"; }
        }
    }
}
