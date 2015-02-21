using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic
{
    public class Display : IDisplay
    {
        public void Output(string text)
        {
            Console.WriteLine(text);
        }

        public string Input()
        {
            return Console.ReadLine();
        }

        public void Clear()
        {
            Console.Clear();
        }
    }
}
