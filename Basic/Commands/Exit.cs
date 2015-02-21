using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.Commands
{
    public class Exit : ICommand
    {
        public void Execute(IInterpreter interpreter)
        {
            interpreter.Exit();
        }
    }
}
