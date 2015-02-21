using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.Errors
{
    public class InterpreterError : Error
    {
        public InterpreterError(string message)
            : base(message)
        {
        }
    }
}
