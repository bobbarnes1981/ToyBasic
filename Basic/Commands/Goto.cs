﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.Commands
{
    public class Goto : Command
    {
        private int m_lineNumber;

        public Goto(int lineNumber)
            : base(Operation.Goto)
        {
            m_lineNumber = lineNumber;
        }

        public override void Execute(IInterpreter interpreter)
        {
            interpreter.Buffer.Jump(m_lineNumber);
        }
    }
}
