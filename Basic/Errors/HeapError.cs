using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.Errors
{
    public class HeapError : Error
    {
        public HeapError(string message)
            : base(message)
        {
        }
    }
}
