namespace Basic.Commands.System
{
    public class List : Command
    {
        public List()
            : base(Keywords.List, true)
        {
        }

        public override void Execute(IInterpreter interpreter)
        {
            interpreter.Buffer.Reset();
            while (!interpreter.Buffer.End)
            {
                interpreter.Buffer.Next();
                interpreter.Console.Output(string.Format("{0}\r\n", interpreter.Buffer.Current));
            }
        }

        public override string Text
        {
            get { return Keywords.List.ToString(); }
        }
    }
}
