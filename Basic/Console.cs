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

        public void Clear()
        {
            System.Console.Clear();
        }
    }
}
