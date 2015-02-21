using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.Errors
{
    public class CommandError : Error
    {
        public CommandError(string message)
            : base(message)
        {
        }
    }
}
