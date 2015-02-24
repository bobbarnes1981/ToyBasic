namespace Basic.Commands.System
{
    public class New : Command
    {
        public New()
            : base(Keywords.New, true)
        {
        }

        public override void Execute(IInterpreter interpreter)
        {
            interpreter.Heap.Clear();
            interpreter.Buffer.Clear();
        }

        public override string Text
        {
            get { return "New"; }
        }
    }
}
