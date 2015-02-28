using Basic.Parsers;

namespace Basic
{
    public class Console : IConsole
    {
        private IParser<object> m_inputParser;

        public Console(IParser<object> inputParser)
        {
            m_inputParser = inputParser;
        }

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
            return m_inputParser.Parse(new TextStream(Input()));
        }

        public void Clear()
        {
            System.Console.Clear();
        }
    }
}
