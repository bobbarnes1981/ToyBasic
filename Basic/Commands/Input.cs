namespace Basic.Commands
{
    public class Input : Command
    {
        private readonly string m_variable;

        public Input(string variable)
            : base(Keyword.Input)
        {
            m_variable = variable;
        }

        public override void Execute(IInterpreter interpreter)
        {
            interpreter.Heap.Set(m_variable, interpreter.Console.Input());
        }

        public override string Text
        {
            get { return string.Format("{0} {1}", Keyword.Input, m_variable); }
        }
    }
}
