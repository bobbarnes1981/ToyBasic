using System.Linq;

namespace Basic
{
    public class Console : IConsole
    {
        public void Output(string text)
        {
            System.Console.Write(text);
        }

        public string Input()
        {
            return System.Console.ReadLine();
        }

        public object ParseInput()
        {
            string input = Input();
            if (input.Any(x => !"0123456789".Contains(x)))
            {
                return input;
            }

            return int.Parse(input);
        }

        public void Clear()
        {
            System.Console.Clear();
        }
    }
}
