namespace Basic.Commands.System
{
    public class Renumber : Command
    {
        public Renumber()
            : base(Keywords.Renumber, true)
        {
        }

        public override void Execute(IInterpreter interpreter)
        {
            interpreter.Buffer.Renumber();
        }

        public override string Text
        {
            get { return Keywords.Renumber.ToString(); }
        }
    }
}
