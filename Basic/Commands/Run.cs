using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.Commands
{
    // not a real command
    public class Run : Command
    {
        public Run()
            : base(Keyword.Run)
        {
        }

        public override void Execute(IInterpreter interpreter)
        {
            interpreter.Execute();
        }

        public override string Text
        {
            get { return "Run"; }
        }
    }
}
