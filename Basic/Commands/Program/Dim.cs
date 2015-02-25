namespace Basic.Commands.Program
{
    public class Dim : Command
    {
        public Dim()
            : base(Keywords.Dim, false)
        {
        }

        public override void Execute(IInterpreter interpreter)
        {
            throw new global::System.NotImplementedException();
        }

        public override string Text
        {
            get { throw new global::System.NotImplementedException(); }
        }
    }
}
