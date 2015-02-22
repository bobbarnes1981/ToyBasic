using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic
{
    public abstract class Command : ICommand
    {
        private Operation m_operation;

        public Command(Operation op)
        {
            m_operation = op;
        }

        public Operation Operation
        {
            get { return m_operation; }
        }

        public abstract void Execute(IInterpreter interpreter);
    }
}
