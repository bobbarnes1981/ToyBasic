using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.Commands
{
    public class Goto : ICommand
    {
        private int m_lineNumber;

        public Goto(int lineNumber)
        {
            m_lineNumber = lineNumber;
        }

        public void Execute(IInterpreter interpreter)
        {
            interpreter.Buffer.Jump(m_lineNumber);
        }
    }
}
