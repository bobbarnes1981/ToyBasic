using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic
{
    public class Program
    {
        static void Main(string[] args)
        {
            new Interpreter(new Buffer(), new Parser(), new Display(), new Heap()).Run();
        }
    }
}
