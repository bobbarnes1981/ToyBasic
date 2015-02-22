using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.Commands
{
    public class Run : Command
    {
        public Run()
            : base(Operation.Run)
        {
        }

        public override void Execute(IInterpreter interpreter)
        {
            interpreter.Execute();
        }
    }
}
