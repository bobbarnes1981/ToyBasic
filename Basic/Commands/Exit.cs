using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.Commands
{
    // not a real command
    public class Exit : Command
    {
        public Exit()
            : base(Keyword.Exit)
        {
        }

        public override void Execute(IInterpreter interpreter)
        {
            interpreter.Exit();
        }

        public override string Text
        {
            get { return "Exit"; }
        }
    }
}
