using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic
{
    public interface IInterpreter
    {
        IBuffer Buffer { get; }
        IDisplay Display { get; }
        IHeap Heap { get; }
        void Execute();
        void Exit();
    }
}
