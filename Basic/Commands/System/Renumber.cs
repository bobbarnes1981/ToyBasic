namespace Basic.Commands.System
{
    public class Renumber : Command
    {
        public Renumber()
            : base(Keyword.Renumber, true)
        {
        }

        public override void Execute(IInterpreter interpreter)
        {
            interpreter.Buffer.Renumber(10);
        }

        public override string Text
        {
            get { return Keyword.Renumber.ToString(); }
        }
    }
}
