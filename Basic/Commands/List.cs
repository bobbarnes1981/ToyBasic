using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.Commands
{
    public class List : Command
    {
        public List()
            : base(Operation.List)
        {
        }

        public override void Execute(IInterpreter interpreter)
        {
            interpreter.Buffer.Reset();
            while(!interpreter.Buffer.End)
            {
                interpreter.Display.Output(interpreter.Buffer.Fetch.ToString());
            }
        }
    }
}
