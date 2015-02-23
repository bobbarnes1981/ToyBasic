namespace Basic.Commands.Program
{
    public class For : Command
    {
        private string m_variable;

        private int m_start;

        private int m_end;

        private int m_step;

        public For(string variable, int start, int end, int step)
            : base(Keyword.For, false)
        {
            m_variable = variable;
            m_start = start;
            m_end = end;
            m_step = step;
        }

        public override void Execute(IInterpreter interpreter)
        {
            IFrame frame = new Frame();
            frame.Set<int>("for_end", m_end);
            frame.Set<int>("for_step", m_step);
            frame.Set<int>("for_line", interpreter.Buffer.Current);
            interpreter.Heap.Set(m_variable, m_start);
            interpreter.Stack.Push(frame);
        }

        public override string Text
        {
            get { return string.Format("{0} {1} = {2} {3} {4} {5} {6}", Keyword.For, m_variable, m_start, Keyword.To, m_end, Keyword.Step, m_step); }
        }
    }
}
