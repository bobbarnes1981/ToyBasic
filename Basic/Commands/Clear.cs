namespace Basic.Commands
{
    public class Clear : Command
    {
        public Clear()
            : base(Keyword.Clear)
        {
        }

        public override void Execute(IInterpreter interpreter)
        {
            interpreter.Console.Clear();
        }

        public override string Text
        {
            get { return Keyword.Clear.ToString(); }
        }
    }
}
