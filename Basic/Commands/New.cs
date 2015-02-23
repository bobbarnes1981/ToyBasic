using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.Commands
{
    // not a real command
    public class New : Command
    {
        public New()
            : base(Keyword.New)
        {
        }

        public override void Execute(IInterpreter interpreter)
        {
            interpreter.Heap.Clear();
            interpreter.Buffer.Clear();
        }

        public override string Text
        {
            get { return "New"; }
        }
    }
}
