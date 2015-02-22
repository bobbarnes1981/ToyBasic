using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic
{
    public interface ICommand
    {
        Operation Operation { get; }
        void Execute(IInterpreter interpreter);
    }
}
