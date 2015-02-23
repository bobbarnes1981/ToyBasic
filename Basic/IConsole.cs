using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic
{
    public interface IConsole
    {
        void Output(string text);
        string Input();
        void Clear();
    }
}
