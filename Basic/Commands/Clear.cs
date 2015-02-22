using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.Commands
{
    public class Clear : Command
    {
        public Clear()
            : base(Operation.Clear)
        {
        }

        public override void Execute(IInterpreter interpreter)
        {
            interpreter.Display.Clear();
        }
    }
}
