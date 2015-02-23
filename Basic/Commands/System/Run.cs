namespace Basic.Commands.System
{
    public class Run : Command
    {
        public Run()
            : base(Keyword.Run, true)
        {
        }

        public override void Execute(IInterpreter interpreter)
        {
            interpreter.Execute();
        }

        public override string Text
        {
            get { return "Run"; }
        }
    }
}
