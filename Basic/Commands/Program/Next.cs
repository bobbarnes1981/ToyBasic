namespace Basic.Commands.Program
{
    public class Next : Command
    {
        private string m_variable;

        public Next(string variable)
            : base(Keyword.Next, false)
        {
            m_variable = variable;
        }

        public override void Execute(IInterpreter interpreter)
        {
            int value = (int)interpreter.Heap.Get(m_variable);
            IFrame frame = interpreter.Stack.Pop();
            if (value < frame.Get<int>("for_end"))
            {
                value += frame.Get<int>("for_step");
                interpreter.Heap.Set(m_variable, value);
                interpreter.Stack.Push(frame);
                interpreter.Buffer.Jump(frame.Get<int>("for_line"));
            }
        }

        public override string Text
        {
            get { return string.Format("{0} {1}", Keyword.Next, m_variable); }
        }
    }
}
