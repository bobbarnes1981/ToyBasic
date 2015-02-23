namespace Basic.Commands
{
    public class List : Command
    {
        public List()
            : base(Keyword.List)
        {
        }

        public override void Execute(IInterpreter interpreter)
        {
            interpreter.Buffer.Reset();
            while(!interpreter.Buffer.End)
            {
                interpreter.Console.Output(string.Format("{0}\r\n", interpreter.Buffer.Fetch));
            }
        }

        public override string Text
        {
            get { return Keyword.List.ToString(); }
        }
    }
}
