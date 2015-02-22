using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.Commands
{
    public class Input : Command
    {
        private string m_variable;

        public Input(string variable)
            : base(Operation.Input)
        {
            m_variable = variable;
        }

        public override void Execute(IInterpreter interpreter)
        {
            interpreter.Heap.Set(m_variable, interpreter.Display.Input());
        }

        public override string Text
        {
            get { return "Input "+m_variable; }
        }
    }
}
