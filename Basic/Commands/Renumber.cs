using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.Commands
{
    public class Renumber : Command
    {
        public Renumber()
            : base(Operation.Renumber)
        {
        }

        public override void Execute(IInterpreter interpreter)
        {
            interpreter.Buffer.Renumber(10);
        }
    }
}
