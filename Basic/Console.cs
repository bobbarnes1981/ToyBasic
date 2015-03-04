using Basic.Parsers;

namespace Basic
{
    public class Console : IConsole
    {
        private IInputParser m_inputParser;

        public Console(IInputParser inputParser)
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
