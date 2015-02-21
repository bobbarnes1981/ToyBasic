using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic
{
    public interface IHeap
    {
        void Set(string variable, object value);
        object Get(string variable);
    }
}
